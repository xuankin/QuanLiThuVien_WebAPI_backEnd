using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApi.Data;
using LibraryManagementApi.Models;
using LibraryManagementApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagermentApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<TacGiaDto>>> GetAuthors()
        {
            return await _context.TacGias
                .Select(t => new TacGiaDto
                {
                    MaTacGia = t.MaTacGia,
                    TenTacGia = t.TenTacGia,
                    NamSinh = t.NamSinh,
                    QuocTich = t.QuocTich
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<TacGiaDto>> GetAuthor(int id)
        {
            var author = await _context.TacGias
                .Select(t => new TacGiaDto
                {
                    MaTacGia = t.MaTacGia,
                    TenTacGia = t.TenTacGia,
                    NamSinh = t.NamSinh,
                    QuocTich = t.QuocTich
                })
                .FirstOrDefaultAsync(t => t.MaTacGia == id);

            if (author == null)
                return NotFound(new { message = $"Author with ID {id} not found." });

            return Ok(author);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<TacGiaDto>> CreateAuthor([FromBody] TacGiaCreateUpdateDto authorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = new TacGia
            {
                TenTacGia = authorDto.TenTacGia,
                NamSinh = authorDto.NamSinh,
                QuocTich = authorDto.QuocTich
            };

            _context.TacGias.Add(author);
            await _context.SaveChangesAsync();

            var createdDto = new TacGiaDto
            {
                MaTacGia = author.MaTacGia,
                TenTacGia = author.TenTacGia,
                NamSinh = author.NamSinh,
                QuocTich = author.QuocTich
            };

            return CreatedAtAction(nameof(GetAuthor), new { id = author.MaTacGia }, createdDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] TacGiaCreateUpdateDto authorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = await _context.TacGias.FindAsync(id);
            if (author == null)
                return NotFound(new { message = $"Author with ID {id} not found." });

            author.TenTacGia = authorDto.TenTacGia;
            author.NamSinh = authorDto.NamSinh;
            author.QuocTich = authorDto.QuocTich;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                    return NotFound(new { message = $"Author with ID {id} not found." });
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.TacGias
                .Include(t => t.SachTacGias)
                .FirstOrDefaultAsync(t => t.MaTacGia == id);
            if (author == null)
                return NotFound(new { message = $"Author with ID {id} not found." });

            if (author.SachTacGias.Any())
                return BadRequest(new { message = "Cannot delete author because they are associated with books." });

            _context.TacGias.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id) => _context.TacGias.Any(t => t.MaTacGia == id);
    }
}