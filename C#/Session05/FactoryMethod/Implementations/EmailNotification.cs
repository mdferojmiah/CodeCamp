using FactoryMethod.Interfaces;

namespace FactoryMethod.Implementations
{
    public class EmailNotification : INotification
    {
        public void NotifyUser(string message)
        {
            Console.WriteLine($"Email Notification: {message}");
        }
    }
}