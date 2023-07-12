using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PageModelExample.Models
{
    public class UserContact
    {
        // Phải có [BindProperty] thì mới thực hiện được việc binding
        [BindProperty(SupportsGet = true)]
        [DisplayName("Id của bạn")]
        [Range(1, 100, ErrorMessage = "{0} nhập không đúng phạm vi từ 1 -> 100")]
        public int UserId { get; set; }

        [BindProperty]
        [DisplayName("Tên người dùng")]
        [Required(ErrorMessage = "{0} không được trống")]
        public string UserName { get; set; }

        [BindProperty]
        [DisplayName("Email của bạn")]
        [Required(ErrorMessage = "{0} không được trống")]
        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; }
    }
}