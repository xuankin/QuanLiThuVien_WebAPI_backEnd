using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Dtos
{
    public class PhieuPhatCreateUpdateDto
    {
        [Required(ErrorMessage = "Mã chi tiết mượn là bắt buộc")]
        public int MaChiTiet { get; set; }

        [Required(ErrorMessage = "Mã nhân viên là bắt buộc")]
        public int MaNhanVien { get; set; }

        public string LyDo { get; set; }
        public string TrangThai { get; set; } // "chưa thanh toán", "đã thanh toán"
    }

    public class PhieuPhatDto
    {
        public int MaPhieuPhat { get; set; }
        public int MaChiTiet { get; set; }
        public int MaNhanVien { get; set; }
        public decimal SoTienPhat { get; set; }
        public string LyDo { get; set; }
        public DateTime NgayPhat { get; set; }
        public string TrangThai { get; set; }
        public string TenNhanVien { get; set; }
    }
}