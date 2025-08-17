using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using LibraryManegermentAPI.Models; // Thêm namespace này để dùng [Key]

namespace LibraryManagementApi.Models // Thêm namespace này
{
    public class NhanVien
    {
        [Key]
        public int MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public int NamSinh { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
    }
}