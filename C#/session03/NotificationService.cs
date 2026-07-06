namespace session03
{
    public class NotificationService
    {
        private readonly List<INotificationChannel> _channels;

        public NotificationService(IEnumerable<INotificationChannel> channels)
        {
            _channels = channels.ToList();
        }

        public void NotifyAll(string message, string recipient)
        {
            foreach (var channel in _channels)
            {
                channel.SendNotification(message, recipient);
            }
        }
    }
}