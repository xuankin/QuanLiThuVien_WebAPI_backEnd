using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApi.Data;
using LibraryManagementApi.Models;
using LibraryManagementApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinesController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private const decimal FinePerDay = 1000;

        public FinesController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<PhieuPhatDto>>> GetFines(int page = 1, int size = 10)
        {
            if (page < 1 || size < 1) return BadRequest("Page and size must be greater than 0.");

            var fines = await _context.PhieuPhats
                .Include(p => p.NhanVien)
                .OrderBy(p => p.MaPhieuPhat)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(p => new PhieuPhatDto
                {
                    MaPhieuPhat = p.MaPhieuPhat,
                    MaChiTiet = p.MaChiTiet,
                    MaNhanVien = p.MaNhanVien,
                    SoTienPhat = p.SoTienPhat,
                    LyDo = p.LyDo,
                    NgayPhat = p.NgayPhat,
                    TrangThai = p.TrangThai,
                    TenNhanVien = p.NhanVien.HoTen
                }).ToListAsync();

            var total = await _context.PhieuPhats.CountAsync();
            Response.Headers.Add("X-Total-Count", total.ToString());
            return Ok(fines);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<PhieuPhatDto>> GetFine(int id)
        {
            var fine = await _context.PhieuPhats
                .Include(p => p.NhanVien)
                .Select(p => new PhieuPhatDto
                {
                    MaPhieuPhat = p.MaPhieuPhat,
                    MaChiTiet = p.MaChiTiet,
                    MaNhanVien = p.MaNhanVien,
                    SoTienPhat = p.SoTienPhat,
                    LyDo = p.LyDo,
                    NgayPhat = p.NgayPhat,
                    TrangThai = p.TrangThai,
                    TenNhanVien = p.NhanVien.HoTen
                })
                .FirstOrDefaultAsync(p => p.MaPhieuPhat == id);

            if (fine == null) return NotFound("Fine not found.");
            return Ok(fine);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<PhieuPhatDto>> CreateFine(PhieuPhatCreateUpdateDto dto)
        {
            var borrowingDetail = await _context.ChiTietMuons
                .Include(c => c.PhieuMuon)
                .FirstOrDefaultAsync(c => c.MaChiTiet == dto.MaChiTiet);

            if (borrowingDetail == null) return NotFound("Borrowing detail not found.");
            if (!_context.NhanViens.Any(n => n.MaNhanVien == dto.MaNhanVien))
                return BadRequest("Invalid employee ID.");

            var dueDate = borrowingDetail.PhieuMuon.NgayPhaiTra;
            var returnDate = borrowingDetail.NgayTra ?? DateTime.Now;
            int daysLate = Math.Max(0, (returnDate - dueDate).Days);

            var fine = new PhieuPhat
            {
                MaChiTiet = dto.MaChiTiet,
                MaNhanVien = dto.MaNhanVien,
                SoTienPhat = daysLate * FinePerDay,
                LyDo = dto.LyDo ?? (daysLate > 0 ? $"Late {daysLate} days" : "No fine applicable"),
                NgayPhat = DateTime.Now,
                TrangThai = dto.TrangThai ?? "chưa thanh toán"
            };

            _context.PhieuPhats.Add(fine);
            await _context.SaveChangesAsync();

            var result = new PhieuPhatDto
            {
                MaPhieuPhat = fine.MaPhieuPhat,
                MaChiTiet = fine.MaChiTiet,
                MaNhanVien = fine.MaNhanVien,
                SoTienPhat = fine.SoTienPhat,
                LyDo = fine.LyDo,
                NgayPhat = fine.NgayPhat,
                TrangThai = fine.TrangThai,
                TenNhanVien = _context.NhanViens.Find(fine.MaNhanVien)?.HoTen
            };

            return CreatedAtAction(nameof(GetFine), new { id = fine.MaPhieuPhat }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateFine(int id, PhieuPhatCreateUpdateDto dto)
        {
            var fine = await _context.PhieuPhats.FindAsync(id);
            if (fine == null) return NotFound("Fine not found.");

            var borrowingDetail = await _context.ChiTietMuons
                .Include(c => c.PhieuMuon)
                .FirstOrDefaultAsync(c => c.MaChiTiet == dto.MaChiTiet);

            if (borrowingDetail == null) return NotFound("Borrowing detail not found.");
            if (!_context.NhanViens.Any(n => n.MaNhanVien == dto.MaNhanVien))
                return BadRequest("Invalid employee ID.");

            var dueDate = borrowingDetail.PhieuMuon.NgayPhaiTra;
            var returnDate = borrowingDetail.NgayTra ?? DateTime.Now;
            int daysLate = Math.Max(0, (returnDate - dueDate).Days);

            fine.MaChiTiet = dto.MaChiTiet;
            fine.MaNhanVien = dto.MaNhanVien;
            fine.SoTienPhat = daysLate * FinePerDay;
            fine.LyDo = dto.LyDo ?? (daysLate > 0 ? $"Late {daysLate} days" : "No fine applicable");
            fine.TrangThai = dto.TrangThai;

            _context.Entry(fine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteFine(int id)
        {
            var fine = await _context.PhieuPhats.FindAsync(id);
            if (fine == null) return NotFound("Fine not found.");

            _context.PhieuPhats.Remove(fine);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}