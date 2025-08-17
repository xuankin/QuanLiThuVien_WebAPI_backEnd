using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementApi.Models
{
    [Table("Sach_TacGia")]
    public class Sach_TacGia
    {
        [Key, Column(Order = 0)]
        public int MaSach { get; set; }

        [Key, Column(Order = 1)]
        public int MaTacGia { get; set; }

        public Sach? Sach { get; set; } // Đánh dấu là nullable

        public TacGia? TacGia { get; set; } // Đánh dấu là nullable
    }
}