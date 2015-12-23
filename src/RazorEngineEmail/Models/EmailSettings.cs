using System.Net.Mail;

namespace RazorEngineEmail.Models
{
    public class EmailSettings
    {
        public string Server { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public string PickupDirectoryLocation { get; set; }
    }
}