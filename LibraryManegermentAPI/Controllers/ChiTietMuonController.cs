using LibraryManagementApi.Data;
using LibraryManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietMuonController : ControllerBase
    {
        private readonly LibraryDbContext _context; // Thay YourDbContext bằng tên DbContext của bạn

        public ChiTietMuonController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/ChiTietMuon - Lấy danh sách tất cả chi tiết mượn
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<ChiTietMuon>>> GetChiTietMuons()
        {
            return await _context.ChiTietMuons
                .Include(ct => ct.PhieuMuon)
                .Include(ct => ct.Sach)
                .ToListAsync();
        }

        // GET: api/ChiTietMuon/5 - Lấy chi tiết mượn theo id
        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<ChiTietMuon>> GetChiTietMuon(int id)
        {
            var chiTietMuon = await _context.ChiTietMuons
                .Include(ct => ct.PhieuMuon)
                .Include(ct => ct.Sach)
                .FirstOrDefaultAsync(ct => ct.MaChiTiet == id);

            if (chiTietMuon == null)
            {
                return NotFound();
            }

            return chiTietMuon;
        }

        // POST: api/ChiTietMuon - Thêm mới chi tiết mượn
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<ChiTietMuon>> CreateChiTietMuon(ChiTietMuon chiTietMuon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ChiTietMuons.Add(chiTietMuon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChiTietMuon), new { id = chiTietMuon.MaChiTiet }, chiTietMuon);
        }

        // PUT: api/ChiTietMuon/5 - Sửa chi tiết mượn
        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateChiTietMuon(int id, ChiTietMuon chiTietMuon)
        {
            if (id != chiTietMuon.MaChiTiet)
            {
                return BadRequest();
            }

            _context.Entry(chiTietMuon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietMuonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ChiTietMuon/5 - Xóa chi tiết mượn
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteChiTietMuon(int id)
        {
            var chiTietMuon = await _context.ChiTietMuons.FindAsync(id);
            if (chiTietMuon == null)
            {
                return NotFound();
            }

            _context.ChiTietMuons.Remove(chiTietMuon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChiTietMuonExists(int id)
        {
            return _context.ChiTietMuons.Any(e => e.MaChiTiet == id);
        }
    }
}