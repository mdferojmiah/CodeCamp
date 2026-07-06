namespace session03
{
    public interface INotificationChannel
    {
        void SendNotification(string message, string recipient);
    }
}