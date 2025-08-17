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
    public class BorrowingSlipsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowingSlipsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<BorrowingSlipResponseDto>>> GetBorrowingSlips(int page = 1, int size = 10)
        {
            if (page < 1 || size < 1) return BadRequest("Page and size must be greater than 0.");

            var slips = await _context.PhieuMuons
                .Include(p => p.DocGia)
                .Include(p => p.NhanVien)
                .OrderBy(p => p.MaPhieuMuon)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(p => new BorrowingSlipResponseDto
                {
                    MaPhieuMuon = p.MaPhieuMuon,
                    MaDocGia = p.MaDocGia,
                    MaNhanVien = p.MaNhanVien,
                    NgayMuon = p.NgayMuon,
                    NgayPhaiTra = p.NgayPhaiTra,
                    TenDocGia = p.DocGia.HoTen,
                    TenNhanVien = p.NhanVien.HoTen,
                    BorrowingDetails = _context.ChiTietMuons
                        .Where(c => c.MaPhieuMuon == p.MaPhieuMuon)
                        .Select(c => new ChiTietMuonCreateUpdateDto
                        {
                            MaPhieuMuon = c.MaPhieuMuon,
                            MaSach = c.MaSach,
                            TinhTrangSach = c.TinhTrangSach
                        }).ToList()
                }).ToListAsync();

            var total = await _context.PhieuMuons.CountAsync();
            Response.Headers.Add("X-Total-Count", total.ToString());
            return Ok(slips);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BorrowingSlipResponseDto>> GetBorrowingSlip(int id)
        {
            var slip = await _context.PhieuMuons
                .Include(p => p.DocGia)
                .Include(p => p.NhanVien)
                .FirstOrDefaultAsync(p => p.MaPhieuMuon == id);

            if (slip == null) return NotFound($"Borrowing slip with ID {id} not found.");

            var details = await _context.ChiTietMuons
                .Where(c => c.MaPhieuMuon == id)
                .Select(c => new ChiTietMuonCreateUpdateDto
                {
                    MaPhieuMuon = c.MaPhieuMuon,
                    MaSach = c.MaSach,
                    TinhTrangSach = c.TinhTrangSach
                }).ToListAsync();

            var result = new BorrowingSlipResponseDto
            {
                MaPhieuMuon = slip.MaPhieuMuon,
                MaDocGia = slip.MaDocGia,
                MaNhanVien = slip.MaNhanVien,
                NgayMuon = slip.NgayMuon,
                NgayPhaiTra = slip.NgayPhaiTra,
                BorrowingDetails = details,
                TenDocGia = slip.DocGia.HoTen,
                TenNhanVien = slip.NhanVien.HoTen
            };

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BorrowingSlipResponseDto>> CreateBorrowingSlip(BorrowingSlipCreateUpdateDto dto)
        {
            if (!_context.DocGias.Any(d => d.MaDocGia == dto.MaDocGia))
                return BadRequest($"Reader with ID {dto.MaDocGia} does not exist.");
            if (!_context.NhanViens.Any(n => n.MaNhanVien == dto.MaNhanVien))
                return BadRequest($"Employee with ID {dto.MaNhanVien} does not exist.");

            var slip = new PhieuMuon
            {
                MaDocGia = dto.MaDocGia,
                MaNhanVien = dto.MaNhanVien,
                NgayMuon = dto.NgayMuon,
                NgayPhaiTra = dto.NgayMuon.AddDays(30)
            };

            _context.PhieuMuons.Add(slip);
            await _context.SaveChangesAsync();

            if (dto.BorrowingDetails == null || !dto.BorrowingDetails.Any())
                return BadRequest("Borrowing details cannot be empty.");
            if (dto.BorrowingDetails.Count > 3)
                return BadRequest("Each borrowing slip can only borrow up to 3 different books.");

            var bookIds = dto.BorrowingDetails.Select(d => d.MaSach).ToList();
            if (bookIds.Distinct().Count() != bookIds.Count)
                return BadRequest("Duplicate book detected in borrowing details.");

            foreach (var detailDto in dto.BorrowingDetails)
            {
                var book = await _context.Saches.FindAsync(detailDto.MaSach);
                if (book == null) return NotFound($"Book with ID {detailDto.MaSach} not found.");
                if (book.SoLuong < 1) return BadRequest($"Book {book.TenSach} is out of stock.");

                book.SoLuong -= 1;
                var detail = new ChiTietMuon
                {
                    MaPhieuMuon = slip.MaPhieuMuon,
                    MaSach = detailDto.MaSach,
                    TinhTrangSach = detailDto.TinhTrangSach ?? "Good"
                };
                _context.ChiTietMuons.Add(detail);
            }

            await _context.SaveChangesAsync();

            var result = new BorrowingSlipResponseDto
            {
                MaPhieuMuon = slip.MaPhieuMuon,
                MaDocGia = slip.MaDocGia,
                MaNhanVien = slip.MaNhanVien,
                NgayMuon = slip.NgayMuon,
                NgayPhaiTra = slip.NgayPhaiTra,
                BorrowingDetails = dto.BorrowingDetails,
                TenDocGia = _context.DocGias.Find(slip.MaDocGia)?.HoTen,
                TenNhanVien = _context.NhanViens.Find(slip.MaNhanVien)?.HoTen
            };

            return CreatedAtAction(nameof(GetBorrowingSlip), new { id = slip.MaPhieuMuon }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateBorrowingSlip(int id, BorrowingSlipCreateUpdateDto dto)
        {
            var slip = await _context.PhieuMuons.FindAsync(id);
            if (slip == null) return NotFound($"Borrowing slip with ID {id} not found.");

            if (!_context.DocGias.Any(d => d.MaDocGia == dto.MaDocGia))
                return BadRequest($"Reader with ID {dto.MaDocGia} does not exist.");
            if (!_context.NhanViens.Any(n => n.MaNhanVien == dto.MaNhanVien))
                return BadRequest($"Employee with ID {dto.MaNhanVien} does not exist.");

            slip.MaDocGia = dto.MaDocGia;
            slip.MaNhanVien = dto.MaNhanVien;
            slip.NgayMuon = dto.NgayMuon;
            slip.NgayPhaiTra = dto.NgayMuon.AddDays(30);

            var existingDetails = await _context.ChiTietMuons.Where(c => c.MaPhieuMuon == id).ToListAsync();
            foreach (var detail in existingDetails)
            {
                var book = await _context.Saches.FindAsync(detail.MaSach);
                if (book != null && detail.NgayTra == null) book.SoLuong += 1;
                _context.ChiTietMuons.Remove(detail);
            }

            if (dto.BorrowingDetails == null || !dto.BorrowingDetails.Any())
                return BadRequest("Borrowing details cannot be empty.");
            if (dto.BorrowingDetails.Count > 3)
                return BadRequest("Each borrowing slip can only borrow up to 3 different books.");

            var bookIds = dto.BorrowingDetails.Select(d => d.MaSach).ToList();
            if (bookIds.Distinct().Count() != bookIds.Count)
                return BadRequest("Duplicate book detected in borrowing details.");

            foreach (var detailDto in dto.BorrowingDetails)
            {
                var book = await _context.Saches.FindAsync(detailDto.MaSach);
                if (book == null) return NotFound($"Book with ID {detailDto.MaSach} not found.");
                if (book.SoLuong < 1) return BadRequest($"Book {book.TenSach} is out of stock.");

                book.SoLuong -= 1;
                var detail = new ChiTietMuon
                {
                    MaPhieuMuon = slip.MaPhieuMuon,
                    MaSach = detailDto.MaSach,
                    TinhTrangSach = detailDto.TinhTrangSach ?? "Good"
                };
                _context.ChiTietMuons.Add(detail);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteBorrowingSlip(int id)
        {
            var slip = await _context.PhieuMuons.FindAsync(id);
            if (slip == null) return NotFound($"Borrowing slip with ID {id} not found.");

            var details = await _context.ChiTietMuons.Where(c => c.MaPhieuMuon == id).ToListAsync();
            foreach (var detail in details)
            {
                var book = await _context.Saches.FindAsync(detail.MaSach);
                if (book != null && detail.NgayTra == null) book.SoLuong += 1;
                _context.ChiTietMuons.Remove(detail);
            }

            _context.PhieuMuons.Remove(slip);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}