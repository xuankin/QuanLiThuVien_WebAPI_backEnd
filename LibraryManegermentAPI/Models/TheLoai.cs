using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // Thêm namespace này để dùng [Key]
namespace LibraryManagementApi.Models // Thêm namespace này
{
    public class TheLoai
    {
        [Key]
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        public ICollection<Sach_TheLoai> SachTheLoais { get; set; } // Quan hệ nhiều-nhiều với Sach
    }
}