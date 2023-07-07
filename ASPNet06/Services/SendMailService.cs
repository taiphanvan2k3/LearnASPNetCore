using Microsoft.Extensions.Options;
using MimeKit;

namespace ASPNet06.Services
{
    public class SendMailService
    {
        private readonly MailSettings _mailSettings;

        public SendMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<string> SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();

            // Không thiết lập email.Sender cũng được. Nếu vậy phải thiết lập ở email.From.Add
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);

            // Chỉ có thể From.Add 1 người thôi, add nhiều thì cũng chỉ hiển thị 1 người gửi thôi.
            // Tên người gửi sẽ phụ thuộc vào From.Add, nhưng email gửi nếu đã thiết lập trong Sender
            // thì đó sẽ là email người gửi, nếu không thiết lập property sender thì tên người gửi và email người gửi
            // sẽ lấy trong email.From
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

            // Tên người nhận không có nên cho tên người nhận chính là email người nhận luôn
            // Có thể gửi mail đến nhiều người
            email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
            email.To.Add(new MailboxAddress("pvt99x@gmail.com", "pvt99x@gmail.com"));
            email.Subject = mailContent.Subject;

            // Do ngoài việc gửi text thì có thể gửi file đính kèm nên dùng 
            var builder = new BodyBuilder();

            builder.HtmlBody = mailContent.Body;
            // builder.Attachments=...

            email.Body = builder.ToMessageBody();

            // Dùng SmtpClient của MailKit
            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                // 3 phương thức dưới đây cũng có phương thức đồng bộ
                await smtpClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtpClient.SendAsync(email);

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return "Loi " + e.Message;
            }

            smtpClient.Disconnect(true);
            return "Gui mail thanh cong";
        }
    }
}