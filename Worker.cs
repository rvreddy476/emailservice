using EmailService.Services;
using System.Net.Mail;
using System.Net;
using EmailService.Models;
using EmailService.MailTemplate;

namespace EmailService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IUserRepo _userRepo;
        private const string FromMail = "****";
        private const string Password = "****";
        private const string SMTPServer = "smtp.office365.com";
        private const int SMPTPort = 587;

        public Worker(ILogger<Worker> logger, IUserRepo userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            var users =await _userRepo.GetPendingReturnsUserDetails();
            foreach (var user in users)
            {
                await SendEmailAsync(user);
            }
             
            
            //}
        }
        private async Task SendEmailAsync(User user)
        {
            try
            {
                var fromAddress = new MailAddress(FromMail, "rajender");
                var toAddress = new MailAddress(user.Email, user.UserName);
                string fromPassword = Password;
                string  body = MailSubject.GetMailSubject(user.UserName,user.GatepassNumber, DateTime.Now.AddDays(1));
                string subject = "This is your daily email.";

                var smtp = new SmtpClient
                {
                    Host = SMTPServer,
                    Port = SMPTPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message); // Send the email asynchronously
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

    }
}
