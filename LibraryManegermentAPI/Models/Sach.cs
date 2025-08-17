using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Models
{
    public class Sach
    {
        [Key]
        public int MaSach { get; set; }

        [Required]
        public string TenSach { get; set; }

        public string NhaXuatBan { get; set; }

        public int SoLuong { get; set; }

        public string ImageUrl { get; set; }

        public List<Sach_TacGia> SachTacGias { get; set; } // Không cần [Required]

        public List<Sach_TheLoai> SachTheLoais { get; set; } // Không cần [Required]
    }
}