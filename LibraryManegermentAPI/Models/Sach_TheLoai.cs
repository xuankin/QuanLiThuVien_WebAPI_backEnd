using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementApi.Models
{
    [Table("Sach_TheLoai")]
    public class Sach_TheLoai
    {
        [Key, Column(Order = 0)]
        public int MaSach { get; set; }

        [Key, Column(Order = 1)]
        public int MaTheLoai { get; set; }

        public Sach? Sach { get; set; } // Đánh dấu là nullable

        public TheLoai? TheLoai { get; set; } // Đánh dấu là nullable
    }
}