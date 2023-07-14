namespace RazorWebTongHop.Services
{
    public class MailSettings
    {
        public string Mail { get; set; }

        public string DisplayName { get; set; }

        // Mật khẩu ứng dụng của gmail
        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }
}