using System.ComponentModel.DataAnnotations;

namespace LibraryManagermentApi.Dtos
{
    public class TheLoaiCreateUpdateDto
    {
        [Required(ErrorMessage = "Tên thể loại là bắt buộc")]
        public string TenTheLoai { get; set; }
    }
}