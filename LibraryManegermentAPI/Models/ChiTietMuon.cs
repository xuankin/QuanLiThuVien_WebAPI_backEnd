using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementApi.Models;

namespace LibraryManagementApi.Models
{
public class ChiTietMuon
{
    [Key] // Đánh dấu khóa chính
    public int MaChiTiet { get; set; }

    public int MaPhieuMuon { get; set; }
    public int MaSach { get; set; }
    public DateTime? NgayTra { get; set; }
    public string TinhTrangSach { get; set; }

    // Navigation properties (tùy chọn)
    public PhieuMuon PhieuMuon { get; set; }
    public Sach Sach { get; set; }
}
}
