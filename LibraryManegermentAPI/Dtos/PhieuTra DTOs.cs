using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Dtos
{
    public class PhieuTraCreateUpdateDto
    {
        [Required(ErrorMessage = "Mã phiếu mượn là bắt buộc")]
        public int MaPhieuMuon { get; set; }

        [Required(ErrorMessage = "Mã nhân viên là bắt buộc")]
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Ngày trả là bắt buộc")]
        public DateTime NgayTra { get; set; }
    }

    public class PhieuTraDto
    {
        public int MaPhieuTra { get; set; }
        public int MaPhieuMuon { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime NgayTra { get; set; }
        public string TenNhanVien { get; set; }
    }
}