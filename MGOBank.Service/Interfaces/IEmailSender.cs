using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Interfaces
{
    public interface IEmailSender
    {
        public void SendEmail(string toEmail, string subject, string body, bool isBodyHtml);
    }
}
