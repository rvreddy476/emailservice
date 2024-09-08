using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Models
{
    public class MailSettings
    {
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort {  get; set; }

    }
}
