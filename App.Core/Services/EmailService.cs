using App.Core.Filters;
using App.Core.ServicesContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    [TypeFilter(typeof(GlobalLoggerFilter))]
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var Client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("Put Your Email address here that send mail to users", "Put yout Pass Key here"),
                EnableSsl = true
            };
            var mail = new MailMessage("emadmahmouddev@gmail.com", to, subject, body);
            await Client.SendMailAsync(mail);
        }
    }
}
