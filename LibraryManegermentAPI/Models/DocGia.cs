using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using LibraryManegermentAPI.Models; // Thêm namespace này để dùng [Key]

namespace LibraryManagementApi.Models
{
    public class DocGia
    {
        [Key]
        public int MaDocGia { get; set; }
        public string HoTen { get; set; }
        public int NamSinh { get; set; }
        public DateTime NgayDangKy { get; set; }
        public DateTime NgayHetHan { get; set; }
    }
}