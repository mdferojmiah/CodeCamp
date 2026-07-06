using session04.Domain;

namespace session04.Interfaces
{
    public interface INotificationService
    {
        void SendNotification(Order order);
    }
}
