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
    public class ReturnSlipsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public ReturnSlipsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<PhieuTraDto>>> GetReturnSlips(int page = 1, int size = 10)
        {
            if (page < 1 || size < 1) return BadRequest("Page and size must be greater than 0.");

            var slips = await _context.PhieuTras
                .Include(p => p.NhanVien)
                .OrderBy(p => p.MaPhieuTra)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(p => new PhieuTraDto
                {
                    MaPhieuTra = p.MaPhieuTra,
                    MaPhieuMuon = p.MaPhieuMuon,
                    MaNhanVien = p.MaNhanVien,
                    NgayTra = p.NgayTra,
                    TenNhanVien = p.NhanVien.HoTen
                }).ToListAsync();

            var total = await _context.PhieuTras.CountAsync();
            Response.Headers.Add("X-Total-Count", total.ToString());
            return Ok(slips);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<PhieuTraDto>> GetReturnSlip(int id)
        {
            var slip = await _context.PhieuTras
                .Include(p => p.NhanVien)
                .Select(p => new PhieuTraDto
                {
                    MaPhieuTra = p.MaPhieuTra,
                    MaPhieuMuon = p.MaPhieuMuon,
                    MaNhanVien = p.MaNhanVien,
                    NgayTra = p.NgayTra,
                    TenNhanVien = p.NhanVien.HoTen
                })
                .FirstOrDefaultAsync(p => p.MaPhieuTra == id);

            if (slip == null) return NotFound("Return slip not found.");
            return Ok(slip);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<PhieuTraDto>> CreateReturnSlip(PhieuTraCreateUpdateDto dto)
        {
            if (!_context.PhieuMuons.Any(p => p.MaPhieuMuon == dto.MaPhieuMuon))
                return BadRequest("Invalid borrowing slip ID.");
            if (!_context.NhanViens.Any(n => n.MaNhanVien == dto.MaNhanVien))
                return BadRequest("Invalid employee ID.");
            if (dto.NgayTra > DateTime.Now)
                return BadRequest("Return date cannot be in the future.");

            var borrowingDetails = await _context.ChiTietMuons
                .Where(c => c.MaPhieuMuon == dto.MaPhieuMuon && c.NgayTra == null)
                .ToListAsync();

            if (!borrowingDetails.Any()) return BadRequest("No unreturned books in this slip.");

            foreach (var detail in borrowingDetails)
            {
                detail.NgayTra = dto.NgayTra;
                var book = await _context.Saches.FindAsync(detail.MaSach);
                if (book != null) book.SoLuong += 1;
            }

            var slip = new PhieuTra
            {
                MaPhieuMuon = dto.MaPhieuMuon,
                MaNhanVien = dto.MaNhanVien,
                NgayTra = dto.NgayTra
            };

            _context.PhieuTras.Add(slip);
            await _context.SaveChangesAsync();

            var result = new PhieuTraDto
            {
                MaPhieuTra = slip.MaPhieuTra,
                MaPhieuMuon = slip.MaPhieuMuon,
                MaNhanVien = slip.MaNhanVien,
                NgayTra = slip.NgayTra,
                TenNhanVien = _context.NhanViens.Find(slip.MaNhanVien)?.HoTen
            };

            return CreatedAtAction(nameof(GetReturnSlip), new { id = slip.MaPhieuTra }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateReturnSlip(int id, PhieuTraCreateUpdateDto dto)
        {
            var slip = await _context.PhieuTras.FindAsync(id);
            if (slip == null) return NotFound("Return slip not found.");

            if (!_context.PhieuMuons.Any(p => p.MaPhieuMuon == dto.MaPhieuMuon))
                return BadRequest("Invalid borrowing slip ID.");
            if (!_context.NhanViens.Any(n => n.MaNhanVien == dto.MaNhanVien))
                return BadRequest("Invalid employee ID.");
            if (dto.NgayTra > DateTime.Now)
                return BadRequest("Return date cannot be in the future.");

            var oldDetails = await _context.ChiTietMuons
                .Where(c => c.MaPhieuMuon == slip.MaPhieuMuon && c.NgayTra == slip.NgayTra)
                .ToListAsync();

            foreach (var detail in oldDetails)
            {
                detail.NgayTra = null;
                var book = await _context.Saches.FindAsync(detail.MaSach);
                if (book != null) book.SoLuong -= 1;
            }

            var newDetails = await _context.ChiTietMuons
                .Where(c => c.MaPhieuMuon == dto.MaPhieuMuon && c.NgayTra == null)
                .ToListAsync();

            if (!newDetails.Any()) return BadRequest("No unreturned books in this slip.");

            foreach (var detail in newDetails)
            {
                detail.NgayTra = dto.NgayTra;
                var book = await _context.Saches.FindAsync(detail.MaSach);
                if (book != null) book.SoLuong += 1;
            }

            slip.MaPhieuMuon = dto.MaPhieuMuon;
            slip.MaNhanVien = dto.MaNhanVien;
            slip.NgayTra = dto.NgayTra;

            _context.Entry(slip).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteReturnSlip(int id)
        {
            var slip = await _context.PhieuTras.FindAsync(id);
            if (slip == null) return NotFound("Return slip not found.");

            var borrowingDetails = await _context.ChiTietMuons
                .Where(c => c.MaPhieuMuon == slip.MaPhieuMuon && c.NgayTra == slip.NgayTra)
                .ToListAsync();

            foreach (var detail in borrowingDetails)
            {
                detail.NgayTra = null;
                var book = await _context.Saches.FindAsync(detail.MaSach);
                if (book != null) book.SoLuong -= 1;
            }

            _context.PhieuTras.Remove(slip);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}