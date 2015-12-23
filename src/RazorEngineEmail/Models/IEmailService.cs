namespace RazorEngineEmail.Models
{
    public interface IEmailService
    {
        void SendEmail(string toEmailAddress, string subject, string body);
        void SendEmail(string[] toEmailAddresses, string subject, string body);
    }
}