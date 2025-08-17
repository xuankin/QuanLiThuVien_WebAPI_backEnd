using System.Collections.Generic;

namespace LibraryManagermentApi.Dtos
{
    public class SachDto
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string NhaXuatBan { get; set; }
        public int SoLuong { get; set; }
        public string ImageUrl { get; set; }
        public List<TacGiaDto> TacGias { get; set; }
        public List<TheLoaiDto> TheLoais { get; set; }
    }
}