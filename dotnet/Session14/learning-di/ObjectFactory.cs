namespace learning_di;

public class ObjectFactory<T>
{
    public static T Get()
    {
        if(typeof(T) == typeof(EmailService))
        {
            return (T)(object)new EmailService();
        }
        if(typeof(T) == typeof(NotificationService))
        {
            var emailService = ObjectFactory<EmailService>.Get();
            return (T)(object)new NotificationService(emailService);
        }
        throw new ArgumentException($"Invalid Key: {typeof(T)}");
    }
}