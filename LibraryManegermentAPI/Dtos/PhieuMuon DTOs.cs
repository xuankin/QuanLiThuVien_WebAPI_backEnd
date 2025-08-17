using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Dtos
{
    public class PhieuMuonDto
    {
        public int MaPhieuMuon { get; set; }
        public int MaDocGia { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime NgayPhaiTra { get; set; }
        public string TenDocGia { get; set; }
        public string TenNhanVien { get; set; }
    }
}