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
    public class EmployeesController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public EmployeesController(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetEmployees()
        {
            return Ok(await _context.NhanViens.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<NhanVien>> GetEmployee(int id)
        {
            var employee = await _context.NhanViens.FirstOrDefaultAsync(n => n.MaNhanVien == id);
            if (employee == null)
            {
                return NotFound(new { Message = $"Employee with ID {id} not found." });
            }
            return Ok(employee);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> SearchEmployees(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { Message = "Search name cannot be empty." });
            }

            var employees = await _context.NhanViens
                .Where(n => n.HoTen.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return Ok(employees);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<NhanVien>> CreateEmployee([FromBody] NhanVien employee)
        {
            if (employee == null)
            {
                return BadRequest(new { Message = "Employee data is required." });
            }

            if (string.IsNullOrWhiteSpace(employee.HoTen) || string.IsNullOrWhiteSpace(employee.SoDienThoai) || string.IsNullOrWhiteSpace(employee.DiaChi))
            {
                return BadRequest(new { Message = "HoTen, SoDienThoai, and DiaChi are required fields." });
            }

            if (employee.NamSinh < 1900 || employee.NamSinh > DateTime.Now.Year)
            {
                return BadRequest(new { Message = "NamSinh must be a valid year." });
            }

            if (employee.NgayVaoLam > DateTime.Now)
            {
                return BadRequest(new { Message = "NgayVaoLam cannot be in the future." });
            }

            _context.NhanViens.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.MaNhanVien }, employee);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] NhanVien employee)
        {
            if (employee == null || id != employee.MaNhanVien)
            {
                return BadRequest(new { Message = "Invalid employee data or ID mismatch." });
            }

            if (string.IsNullOrWhiteSpace(employee.HoTen) || string.IsNullOrWhiteSpace(employee.SoDienThoai) || string.IsNullOrWhiteSpace(employee.DiaChi))
            {
                return BadRequest(new { Message = "HoTen, SoDienThoai, and DiaChi are required fields." });
            }

            if (employee.NamSinh < 1900 || employee.NamSinh > DateTime.Now.Year)
            {
                return BadRequest(new { Message = "NamSinh must be a valid year." });
            }

            if (employee.NgayVaoLam > DateTime.Now)
            {
                return BadRequest(new { Message = "NgayVaoLam cannot be in the future." });
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound(new { Message = $"Employee with ID {id} not found." });
                }
                return Conflict(new { Message = "Concurrency conflict occurred. The employee data may have been modified by another user." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.NhanViens.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new { Message = $"Employee with ID {id} not found." });
            }

            _context.NhanViens.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id) => _context.NhanViens.Any(n => n.MaNhanVien == id);
    }
}