using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApi.Data;
using LibraryManagementApi.Models;
using LibraryManagementApi.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagermentApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SachDto>>> GetBooks()
        {
            return await _context.Saches
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    NhaXuatBan = s.NhaXuatBan,
                    SoLuong = s.SoLuong,
                    ImageUrl = s.ImageUrl,
                    TacGias = s.SachTacGias.Select(st => new TacGiaDto
                    {
                        MaTacGia = st.MaTacGia,
                        TenTacGia = st.TacGia.TenTacGia,
                        NamSinh = st.TacGia.NamSinh,
                        QuocTich = st.TacGia.QuocTich
                    }).ToList(),
                    TheLoais = s.SachTheLoais.Select(st => new TheLoaiDto
                    {
                        MaTheLoai = st.MaTheLoai,
                        TenTheLoai = st.TheLoai.TenTheLoai
                    }).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<SachDto>> GetBook(int id)
        {
            var book = await _context.Saches
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    NhaXuatBan = s.NhaXuatBan,
                    SoLuong = s.SoLuong,
                    ImageUrl = s.ImageUrl,
                    TacGias = s.SachTacGias.Select(st => new TacGiaDto
                    {
                        MaTacGia = st.MaTacGia,
                        TenTacGia = st.TacGia.TenTacGia,
                        NamSinh = st.TacGia.NamSinh,
                        QuocTich = st.TacGia.QuocTich
                    }).ToList(),
                    TheLoais = s.SachTheLoais.Select(st => new TheLoaiDto
                    {
                        MaTheLoai = st.MaTheLoai,
                        TenTheLoai = st.TheLoai.TenTheLoai
                    }).ToList()
                })
                .FirstOrDefaultAsync(s => s.MaSach == id);

            if (book == null) return NotFound();
            return book;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SachDto>>> SearchBooks(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Search term is required.");

            return await _context.Saches
                .Where(s => s.TenSach.Contains(name))
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    NhaXuatBan = s.NhaXuatBan,
                    SoLuong = s.SoLuong,
                    ImageUrl = s.ImageUrl,
                    TacGias = s.SachTacGias.Select(st => new TacGiaDto
                    {
                        MaTacGia = st.MaTacGia,
                        TenTacGia = st.TacGia.TenTacGia,
                        NamSinh = st.TacGia.NamSinh,
                        QuocTich = st.TacGia.QuocTich
                    }).ToList(),
                    TheLoais = s.SachTheLoais.Select(st => new TheLoaiDto
                    {
                        MaTheLoai = st.MaTheLoai,
                        TenTheLoai = st.TheLoai.TenTheLoai
                    }).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("by-author/{authorId}")]
        public async Task<ActionResult<IEnumerable<SachDto>>> GetBooksByAuthor(int authorId)
        {
            if (authorId <= 0) return BadRequest("Invalid author ID.");

            var books = await _context.SachTacGias
                .Where(st => st.MaTacGia == authorId)
                .Select(st => st.Sach)
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    NhaXuatBan = s.NhaXuatBan,
                    SoLuong = s.SoLuong,
                    ImageUrl = s.ImageUrl,
                    TacGias = s.SachTacGias.Select(st => new TacGiaDto
                    {
                        MaTacGia = st.MaTacGia,
                        TenTacGia = st.TacGia.TenTacGia,
                        NamSinh = st.TacGia.NamSinh,
                        QuocTich = st.TacGia.QuocTich
                    }).ToList(),
                    TheLoais = s.SachTheLoais.Select(st => new TheLoaiDto
                    {
                        MaTheLoai = st.MaTheLoai,
                        TenTheLoai = st.TheLoai.TenTheLoai
                    }).ToList()
                })
                .ToListAsync();

            if (!books.Any()) return NotFound("No books found for this author.");
            return books;
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<SachDto>>> GetBooksByCategory(int categoryId)
        {
            if (categoryId <= 0) return BadRequest("Invalid category ID.");

            var books = await _context.SachTheLoais
                .Where(st => st.MaTheLoai == categoryId)
                .Select(st => st.Sach)
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    NhaXuatBan = s.NhaXuatBan,
                    SoLuong = s.SoLuong,
                    ImageUrl = s.ImageUrl,
                    TacGias = s.SachTacGias.Select(st => new TacGiaDto
                    {
                        MaTacGia = st.MaTacGia,
                        TenTacGia = st.TacGia.TenTacGia,
                        NamSinh = st.TacGia.NamSinh,
                        QuocTich = st.TacGia.QuocTich
                    }).ToList(),
                    TheLoais = s.SachTheLoais.Select(st => new TheLoaiDto
                    {
                        MaTheLoai = st.MaTheLoai,
                        TenTheLoai = st.TheLoai.TenTheLoai
                    }).ToList()
                })
                .ToListAsync();

            if (!books.Any()) return NotFound("No books found for this category.");
            return books;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<SachDto>> CreateBook([FromBody] SachCreateUpdateDto bookDto)
        {
            if (bookDto == null) return BadRequest("Book data is required.");

            var authorIds = bookDto.MaTacGias.Distinct().ToList();
            var invalidAuthorIds = authorIds.Where(id => id <= 0 || !_context.TacGias.Any(t => t.MaTacGia == id)).ToList();
            if (invalidAuthorIds.Any())
                return BadRequest($"Invalid or non-existent author IDs: {string.Join(", ", invalidAuthorIds)}");

            var categoryIds = bookDto.MaTheLoais.Distinct().ToList();
            var invalidCategoryIds = categoryIds.Where(id => id <= 0 || !_context.TheLoais.Any(t => t.MaTheLoai == id)).ToList();
            if (invalidCategoryIds.Any())
                return BadRequest($"Invalid or non-existent category IDs: {string.Join(", ", invalidCategoryIds)}");

            var newBook = new Sach
            {
                TenSach = bookDto.TenSach,
                NhaXuatBan = bookDto.NhaXuatBan,
                SoLuong = bookDto.SoLuong,
                ImageUrl = bookDto.ImageUrl
            };

            _context.Saches.Add(newBook);
            await _context.SaveChangesAsync();

            var sachTacGias = authorIds.Select(maTacGia => new Sach_TacGia
            {
                MaSach = newBook.MaSach,
                MaTacGia = maTacGia
            }).ToList();

            var sachTheLoais = categoryIds.Select(maTheLoai => new Sach_TheLoai
            {
                MaSach = newBook.MaSach,
                MaTheLoai = maTheLoai
            }).ToList();

            _context.SachTacGias.AddRange(sachTacGias);
            _context.SachTheLoais.AddRange(sachTheLoais);
            await _context.SaveChangesAsync();

            var createdBook = await _context.Saches
                .Where(s => s.MaSach == newBook.MaSach)
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    NhaXuatBan = s.NhaXuatBan,
                    SoLuong = s.SoLuong,
                    ImageUrl = s.ImageUrl,
                    TacGias = s.SachTacGias.Select(st => new TacGiaDto
                    {
                        MaTacGia = st.MaTacGia,
                        TenTacGia = st.TacGia.TenTacGia,
                        NamSinh = st.TacGia.NamSinh,
                        QuocTich = st.TacGia.QuocTich
                    }).ToList(),
                    TheLoais = s.SachTheLoais.Select(st => new TheLoaiDto
                    {
                        MaTheLoai = st.MaTheLoai,
                        TenTheLoai = st.TheLoai.TenTheLoai
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetBook), new { id = newBook.MaSach }, createdBook);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] SachCreateUpdateDto bookDto)
        {
            if (bookDto == null) return BadRequest("Book data is required.");

            var existingBook = await _context.Saches
                .Include(s => s.SachTacGias)
                .Include(s => s.SachTheLoais)
                .FirstOrDefaultAsync(s => s.MaSach == id);
            if (existingBook == null) return NotFound();

            var authorIds = bookDto.MaTacGias.Distinct().ToList();
            var invalidAuthorIds = authorIds.Where(id => id <= 0 || !_context.TacGias.Any(t => t.MaTacGia == id)).ToList();
            if (invalidAuthorIds.Any())
                return BadRequest($"Invalid or non-existent author IDs: {string.Join(", ", invalidAuthorIds)}");

            var categoryIds = bookDto.MaTheLoais.Distinct().ToList();
            var invalidCategoryIds = categoryIds.Where(id => id <= 0 || !_context.TheLoais.Any(t => t.MaTheLoai == id)).ToList();
            if (invalidCategoryIds.Any())
                return BadRequest($"Invalid or non-existent category IDs: {string.Join(", ", invalidCategoryIds)}");

            existingBook.TenSach = bookDto.TenSach;
            existingBook.NhaXuatBan = bookDto.NhaXuatBan;
            existingBook.SoLuong = bookDto.SoLuong;
            existingBook.ImageUrl = bookDto.ImageUrl;

            _context.SachTacGias.RemoveRange(existingBook.SachTacGias);
            _context.SachTheLoais.RemoveRange(existingBook.SachTheLoais);

            var newSachTacGias = authorIds.Select(maTacGia => new Sach_TacGia
            {
                MaSach = id,
                MaTacGia = maTacGia
            }).ToList();

            var newSachTheLoais = categoryIds.Select(maTheLoai => new Sach_TheLoai
            {
                MaSach = id,
                MaTheLoai = maTheLoai
            }).ToList();

            _context.SachTacGias.AddRange(newSachTacGias);
            _context.SachTheLoais.AddRange(newSachTheLoais);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Saches
                .Include(s => s.SachTacGias)
                .Include(s => s.SachTheLoais)
                .FirstOrDefaultAsync(s => s.MaSach == id);
            if (book == null) return NotFound();

            _context.SachTacGias.RemoveRange(book.SachTacGias);
            _context.SachTheLoais.RemoveRange(book.SachTheLoais);
            _context.Saches.Remove(book);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool BookExists(int id) => _context.Saches.Any(s => s.MaSach == id);
    }
}