using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // Thêm namespace này để dùng [Key]
namespace LibraryManagementApi.Models // Thêm namespace này
{
public class PhieuMuon
{
    [Key]
    public int MaPhieuMuon { get; set; }
    public int MaDocGia { get; set; }
    public int MaNhanVien { get; set; }
    public DateTime NgayMuon { get; set; }
    public DateTime NgayPhaiTra { get; set; }

    public DocGia DocGia { get; set; }
    public NhanVien NhanVien { get; set; }
}
}