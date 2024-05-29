namespace english_learning_server.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}