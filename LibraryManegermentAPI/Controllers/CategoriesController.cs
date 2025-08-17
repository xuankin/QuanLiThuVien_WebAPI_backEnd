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
    public class CategoriesController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public CategoriesController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<TheLoaiDto>>> GetCategories()
        {
            return await _context.TheLoais
                .Select(t => new TheLoaiDto
                {
                    MaTheLoai = t.MaTheLoai,
                    TenTheLoai = t.TenTheLoai
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<TheLoaiDto>> GetCategory(int id)
        {
            var category = await _context.TheLoais
                .Select(t => new TheLoaiDto
                {
                    MaTheLoai = t.MaTheLoai,
                    TenTheLoai = t.TenTheLoai
                })
                .FirstOrDefaultAsync(t => t.MaTheLoai == id);

            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found." });

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<TheLoaiDto>> CreateCategory([FromBody] TheLoaiCreateUpdateDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new TheLoai
            {
                TenTheLoai = categoryDto.TenTheLoai
            };

            _context.TheLoais.Add(category);
            await _context.SaveChangesAsync();

            var createdDto = new TheLoaiDto
            {
                MaTheLoai = category.MaTheLoai,
                TenTheLoai = category.TenTheLoai
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.MaTheLoai }, createdDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] TheLoaiCreateUpdateDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _context.TheLoais.FindAsync(id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found." });

            category.TenTheLoai = categoryDto.TenTheLoai;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                    return NotFound(new { message = $"Category with ID {id} not found." });
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.TheLoais
                .Include(t => t.SachTheLoais)
                .FirstOrDefaultAsync(t => t.MaTheLoai == id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found." });

            if (category.SachTheLoais.Any())
                return BadRequest(new { message = "Cannot delete category because it is associated with books." });

            _context.TheLoais.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id) => _context.TheLoais.Any(t => t.MaTheLoai == id);
    }
}