using session04.Domain;
using session04.Interfaces;

namespace session04.Services
{
    public class PushNotificationService : INotificationService
    {
        public void SendNotification(Order order)
        {
            Console.WriteLine($"[PushNotification] Order {order.Id} for {order.Item} has been processed with amount {order.Amount}.");
        }
    }
}