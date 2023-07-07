using System.Net;
using System.Net.Mail;
using System.Text;

namespace ASPNet06.MailUtils
{
    public class MailUtils
    {
        // Trường hợp gửi mail này yêu cầu máy đang chạy ứng dụng này phải
        // có Mail Transporter. Do đó phương thức này không nên dùng vì độ phổ biến ít
        public static async Task<string> SendMail(string from, string to, string subject, string body)
        {
            // Sẽ gửi mail từ máy chủ localhost
            MailMessage message = new MailMessage(from, to, subject, body);
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;

            // Nếu nội dung cho phép trình bày theo HTML:
            message.IsBodyHtml = true;

            // Khi người dùng bấm dùng reply thì gửi đến địa chỉ nào
            message.ReplyToList.Add(new MailAddress(from));
            message.Sender = new MailAddress(from, "Tài Phan Văn");

            // message này sẽ được SmtpClient kết nối với server để gửi đi
            using var smtpClient = new SmtpClient("localhost");
            try
            {
                await smtpClient.SendMailAsync(message);
                return "Gui email thanh cong";
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return "Gui email that bai";
            }
        }

        // Phương thức này dùng được nhưng ưu tiên sử dụng phương thức SendEmail của SendMailService
        public static async Task<string> SendGmail(string from, string to, string subject, string body, 
                                                    string password)
        {
            // Sẽ gửi mail từ máy chủ localhost
            MailMessage message = new MailMessage(from, to, subject, body);
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;

            // Nếu nội dung cho phép trình bày theo HTML:
            message.IsBodyHtml = true;

            // Khi người dùng bấm dùng reply thì gửi đến địa chỉ nào
            message.ReplyToList.Add(new MailAddress(from));
            message.Sender = new MailAddress(from, "Tài Phan Văn");

            // message này sẽ được SmtpClient kết nối với server để gửi đi
            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(from, password);
            try
            {
                await smtpClient.SendMailAsync(message);
                return "Gui email thanh cong";
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return "Gui email that bai";
            }
        }
    }
}