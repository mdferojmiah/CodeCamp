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

public interface ISingletonService
{
    void Print();
}

public class SingletonService : ISingletonService
{
    private Guid guid;
    public SingletonService()
    {
        guid = Guid.NewGuid();
    }
    public void Print()
    {
        Console.WriteLine($"Singleton Instance: {guid}");
    }
}

public interface ITransientService
{
    void Print();
}

public class TransientService : ITransientService
{
    private Guid guid;
    public TransientService()
    {
        guid = Guid.NewGuid();
    }
    public void Print()
    {
        Console.WriteLine($"Transient Instance: {guid}");
    }
}

public interface IScopedService
{
    void Print();
}

public class ScopedService : IScopedService
{
    private Guid guid;
    public ScopedService()
    {
        guid = Guid.NewGuid();
    }
    public void Print()
    {
        Console.WriteLine($"Scoped Instance: {guid}");
    }
}