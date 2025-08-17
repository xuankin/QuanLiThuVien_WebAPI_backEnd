using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Dtos
{
    public class TacGiaCreateUpdateDto
    {
        [Required(ErrorMessage = "Tên tác giả là bắt buộc")]
        public string TenTacGia { get; set; }

        public int? NamSinh { get; set; }

        public string QuocTich { get; set; }
    }
}