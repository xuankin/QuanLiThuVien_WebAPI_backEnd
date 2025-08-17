using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // Thêm namespace này để dùng [Key]
namespace LibraryManagementApi.Models // Thêm namespace này
{
    public class TacGia
    {
        [Key]
        public int MaTacGia { get; set; }
        public string TenTacGia { get; set; }
        public int? NamSinh { get; set; }
        public string QuocTich { get; set; }
        public ICollection<Sach_TacGia> SachTacGias { get; set; } // Quan hệ nhiều-nhiều với Sach
    }
}