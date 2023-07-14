using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace RazorWebTongHop.Services
{
    /// <summary>
    /// Chú ý phải implement IEmailSender
    /// </summary>
    public class SendMailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        public SendMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            // Do ngoài việc gửi text thì có thể gửi file đính kèm nên dùng
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;

            message.Body = builder.ToMessageBody();

            // Dùng SmtpClient của Mailkit
            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtpClient.SendAsync(message);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Gui email that bai:" + e.Message);
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                if (!Directory.Exists("MailSave"))
                {
                    Directory.CreateDirectory("MailSave");
                    var emailSaveFile = string.Format(@"MailSave/{0}.eml", Guid.NewGuid());
                    await message.WriteToAsync(emailSaveFile);
                    System.Console.WriteLine("Loi gui email, luu tai " + emailSaveFile);
                }
            }
            smtpClient.Disconnect(true);
            System.Console.WriteLine("Gui email thanh cong");
        }
    }
}