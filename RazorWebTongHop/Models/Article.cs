using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorWebTongHop.Models
{
    public class Article
    {
        public int Id { get; set; }

        [StringLength(255, MinimumLength = 5, ErrorMessage = "{0} phải dài từ {2} đến {1}")]
        [Required(ErrorMessage = "{0} phải nhập")]
        [Column(TypeName = "nvarchar")] // mặc định là nvarchar rồi
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "{0} phải tạo")]
        [DisplayName("Ngày tạo")]
        public DateTime CreateAt { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }
    }
}