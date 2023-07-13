using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorWebTongHop.Models
{
    public class Article
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        [Column(TypeName = "nvarchar")] // mặc định là nvarchar rồi
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime CreateAt { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }
    }
}