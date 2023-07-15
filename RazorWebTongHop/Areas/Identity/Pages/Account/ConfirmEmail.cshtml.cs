// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ConfirmEmailModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Phương thức này sẽ được gọi khi người dùng nhấn vào url trong gửi ở email.
        /// Khi nhấn vào url sẽ tiến hành binding các tham số trên query vào userId, code
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            // Tìm user theo userId, kiểu dữ liệu trả về là 1 đối tượng AppUser
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Không tìm thấy User có Id = '{userId}'.");
            }

            // Lần trước gửi email đi đã encode mã token bằng base64, thì giờ nhận lại
            // phải decode trở lại
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            // So khớp thông tin user và mã token
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Email đã được xác thực." : "Lỗi xác thực email.";

            /* Nếu thành công thì đăng nhập luôn, do đó cần inject SignInManager vào PageModel
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }
            else
            {
                return Content("Lỗi xác thực email.");
            } 
            */

            // Hoặc hiển thị đã xác nhận email thành công.
            return Page();
        }
    }
}
