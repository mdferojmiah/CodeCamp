using FactoryMethod.Factories;
using FactoryMethod.Interfaces;

namespace FactoryMethod
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("-----Factory Method Pattern-----");

            NotificationFactory factory = new EmailFactory();
            INotification emailNotification = factory.CreateNotification();
            emailNotification.NotifyUser("This is an email notification.");
            Console.WriteLine();

            factory = new SmsFactory();
            INotification smsNotification = factory.CreateNotification();
            smsNotification.NotifyUser("This is an SMS notification.");
        }
    }
}