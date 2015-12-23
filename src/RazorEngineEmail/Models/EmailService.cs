using System;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace RazorEngineEmail.Models
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IConfiguration config)
        {
            var config1 = config;
            _settings = new EmailSettings()
            {
                Server = config1["Data:EmailSettings:Server"],
                EnableSsl = Convert.ToBoolean(config1["Data:EmailSettings:EnableSsl"]),
                Port = Convert.ToInt16(config1["Data:EmailSettings:Port"]),
                UserName = config1["Data:EmailSettings:UserName"],
                Password = config1["Data:EmailSettings:Password"],
                DisplayName = config1["Data:EmailSettings:DisplayName"],
                DeliveryMethod = config1["Data:EmailSettings:SmtpDeliveryMethod"] == "SpecifiedPickupDirectory" ? SmtpDeliveryMethod.SpecifiedPickupDirectory : SmtpDeliveryMethod.Network,
                PickupDirectoryLocation = config1["Data:EmailSettings:PickupDirectoryLocation"]
            };
        }

        public void SendEmail(string toEmailAddress, string subject, string body)
        {
            SendEmail(new[] { toEmailAddress }, subject, body);
        }

        public void SendEmail(string[] toEmailAddresses, string subject, string body)
        {
            MailMessage message = new MailMessage();
            MailAddress sender = new MailAddress(_settings.UserName, _settings.DisplayName);

            SmtpClient smtp;
            if (_settings.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                smtp = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = _settings.PickupDirectoryLocation
                };
            }
            else
            {
                smtp = new SmtpClient()
                {
                    Host = _settings.Server,
                    Port = _settings.Port,
                    EnableSsl = _settings.EnableSsl,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(_settings.UserName, _settings.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
            }
            message.From = sender;

            foreach (var strEmail in toEmailAddresses)
                message.To.Add(new MailAddress(strEmail.Trim()));

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            smtp.Send(message);
        }

    }
}