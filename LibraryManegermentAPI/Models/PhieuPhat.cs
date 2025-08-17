using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // Thêm namespace này để dùng [Key]
namespace LibraryManagementApi.Models // Thêm namespace này
{
public class PhieuPhat
{
    [Key]   
    public int MaPhieuPhat { get; set; }
    public int MaChiTiet { get; set; }
    public int MaNhanVien { get; set; }
    public decimal SoTienPhat { get; set; }
    public string? LyDo { get; set; }
    public DateTime NgayPhat { get; set; }
    public string? TrangThai { get; set; } // "chưa thanh toán", "đã thanh toán"

    public ChiTietMuon ChiTietMuon { get; set; }
    public NhanVien NhanVien { get; set; }
}
}