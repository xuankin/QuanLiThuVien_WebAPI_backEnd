namespace LibraryManagementApi.Dtos
{
    // DTO cho request khi tạo hoặc cập nhật
    public class BorrowingSlipCreateUpdateDto
    {
        public int MaDocGia { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime NgayMuon { get; set; }
        public List<ChiTietMuonCreateUpdateDto> BorrowingDetails { get; set; } = new List<ChiTietMuonCreateUpdateDto>();
    }

    // DTO cho response
    public class BorrowingSlipResponseDto
    {
        public int MaPhieuMuon { get; set; }
        public int MaDocGia { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime NgayPhaiTra { get; set; }
        public List<ChiTietMuonCreateUpdateDto> BorrowingDetails { get; set; } = new List<ChiTietMuonCreateUpdateDto>();
        public string TenDocGia { get; set; }
        public string TenNhanVien { get; set; }
    }

    // DTO cho chi tiết mượn (giữ nguyên)
    public class ChiTietMuonCreateUpdateDto
    {
        public int? MaPhieuMuon { get; set; } // Nullable cho response, không cần trong request
        public int MaSach { get; set; }
        public string TinhTrangSach { get; set; }
    }
}