using FactoryMethod.Interfaces;

namespace FactoryMethod.Implementations
{
    public class SmsNotification : INotification
    {
        public void NotifyUser(string message)
        {
            Console.WriteLine($"SMS Notification: {message}");
        }
    }
}