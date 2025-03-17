using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace TimeFourthe.Mails
{
    public class MailSender
    {
        public static async Task SendMail(string[] recipients, string html, string title,string senderName="Ketul")
        {
            string senderEmail = "timefourthe@gmail.com";
            // string senderName="Web University";
            string senderPassword = "hcqi fyxs hawx gxcx"; // Use an App Password for Gmail
            try
            {
                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(senderEmail, senderPassword);

                    foreach (var recipient in recipients)
                    {
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress(senderName, senderEmail));
                        message.To.Add(new MailboxAddress("Recipient", recipient));
                        // message.Subject = "Your One-Time Password (OTP) 🔐";
                        message.Subject = title;

                        // HTML OTP Email Body (Dark Theme)
                        var body = $@"{html}";
                        message.Body = new TextPart("html") { Text = body };
                        await smtp.SendAsync(message);
                    }
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}