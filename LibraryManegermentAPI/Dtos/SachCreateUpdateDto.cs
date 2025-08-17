using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Dtos
{
    public class SachCreateUpdateDto
    {
        [Required(ErrorMessage = "Tên sách là bắt buộc")]
        public string TenSach { get; set; }

        public string NhaXuatBan { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int SoLuong { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Phải có ít nhất một tác giả")]
        public List<int> MaTacGias { get; set; }

        [Required(ErrorMessage = "Phải có ít nhất một thể loại")]
        public List<int> MaTheLoais { get; set; }
    }
}