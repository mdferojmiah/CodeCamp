namespace learning_di;

public class NotificationService(IEmailService emailService)
{
    public void Notify(string User, string message)
    {
        emailService.SendEmail(User, message);
    }
}

public interface IEmailService
{
    void SendEmail(string User, string message);
}

public class EmailService : IEmailService
{
    public void SendEmail(string User, string message)
    {
        Console.WriteLine($"Sending Email to: {User}");
        Console.WriteLine($"Email body: {message}");
    }
}