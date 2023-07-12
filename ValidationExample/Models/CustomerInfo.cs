using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ValidationExample.Binders;
using ValidationExample.Validations;

namespace ValidationExample.Models
{
    public class CustomerInfo
    {
        [DisplayName("Tên khách hàng")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} không thỏa mãn chiều dài {2} -> {1} kí tự")]
        [Required(ErrorMessage = "Phải nhập {0}.")]
        [ModelBinder(BinderType = typeof(UserNameBinding))]
        public string CustomerName { get; set; }

        [DisplayName("Địa chỉ email")]
        [EmailAddress(ErrorMessage = "{0} không phù hợp")]
        [Required(ErrorMessage = "Phải nhập địa chỉ email")]
        public string Email { get; set; }

        [Display(Name = "Năm sinh")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [Range(1970, 2000, ErrorMessage = "{0} sai, phải nằm trong khoảng {1} -> {2}")]
        [EvenNumber]
        public int? YearOfBirth { get; set; }
    }
}