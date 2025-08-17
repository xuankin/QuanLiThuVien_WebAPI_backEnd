using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApi.Data;
using LibraryManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadersController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public ReadersController(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<DocGia>>> GetReaders()
        {
            return Ok(await _context.DocGias.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]

        public async Task<ActionResult<DocGia>> GetReader(int id)
        {
            var reader = await _context.DocGias.FirstOrDefaultAsync(d => d.MaDocGia == id);
            if (reader == null)
            {
                return NotFound(new { Message = $"Reader with ID {id} not found." });
            }
            return Ok(reader);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Employee")]

        public async Task<ActionResult<IEnumerable<DocGia>>> SearchReaders(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { Message = "Search name cannot be empty." });
            }

            var readers = await _context.DocGias
                .Where(d => d.HoTen.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return Ok(readers);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<DocGia>> CreateReader([FromBody] DocGia reader)
        {
            if (reader == null)
            {
                return BadRequest(new { Message = "Reader data is required." });
            }

            if (string.IsNullOrWhiteSpace(reader.HoTen))
            {
                return BadRequest(new { Message = "HoTen is a required field." });
            }

            if (reader.NamSinh < 1900 || reader.NamSinh > DateTime.Now.Year)
            {
                return BadRequest(new { Message = "NamSinh must be a valid year." });
            }

            if (reader.NgayDangKy > DateTime.Now)
            {
                return BadRequest(new { Message = "NgayDangKy cannot be in the future." });
            }

            if (reader.NgayHetHan <= reader.NgayDangKy)
            {
                return BadRequest(new { Message = "NgayHetHan must be later than NgayDangKy." });
            }

            _context.DocGias.Add(reader);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReader), new { id = reader.MaDocGia }, reader);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateReader(int id, [FromBody] DocGia reader)
        {
            if (reader == null || id != reader.MaDocGia)
            {
                return BadRequest(new { Message = "Invalid reader data or ID mismatch." });
            }

            if (string.IsNullOrWhiteSpace(reader.HoTen))
            {
                return BadRequest(new { Message = "HoTen is a required field." });
            }

            if (reader.NamSinh < 1900 || reader.NamSinh > DateTime.Now.Year)
            {
                return BadRequest(new { Message = "NamSinh must be a valid year." });
            }

            if (reader.NgayDangKy > DateTime.Now)
            {
                return BadRequest(new { Message = "NgayDangKy cannot be in the future." });
            }

            if (reader.NgayHetHan <= reader.NgayDangKy)
            {
                return BadRequest(new { Message = "NgayHetHan must be later than NgayDangKy." });
            }

            _context.Entry(reader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReaderExists(id))
                {
                    return NotFound(new { Message = $"Reader with ID {id} not found." });
                }
                return Conflict(new { Message = "Concurrency conflict occurred. The reader data may have been modified by another user." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.DocGias.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { Message = $"Reader with ID {id} not found." });
            }

            _context.DocGias.Remove(reader);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("{id}/extend")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ExtendReaderExpiration(int id)
        {
            var reader = await _context.DocGias.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { Message = $"Reader with ID {id} not found." });
            }

            // Gia hạn thêm 30 ngày
            reader.NgayHetHan = reader.NgayHetHan.AddDays(30);

            _context.Entry(reader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReaderExists(id))
                {
                    return NotFound(new { Message = $"Reader with ID {id} not found." });
                }
                return Conflict(new { Message = "Concurrency conflict occurred. The reader data may have been modified by another user." });
            }

            return Ok(new { Message = "Expiration date extended successfully.", NewExpirationDate = reader.NgayHetHan });
        }

        private bool ReaderExists(int id) => _context.DocGias.Any(d => d.MaDocGia == id);
    }
}