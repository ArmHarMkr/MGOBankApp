using MGOBank.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Implementations
{
    public class EmailSender : IEmailSender
    {
        private string fromEmail = "metalcutdotnet@gmail.com";
        private string fromPass = "oiuapjjwnbsxfhqn";

        public void SendEmail(string toEmail, string subject, string body, bool isBodyHtml)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromEmail);
            mailMessage.To.Add(new MailAddress(toEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = isBodyHtml;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPass),
                EnableSsl = true,
            };

            smtpClient.Send(mailMessage);
        }
    }
}
