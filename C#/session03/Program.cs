namespace session03
{
    public class Program
    {
        public static void Main(string[] args)
        {
            INotificationChannel emailChannel = new EmailChannel();
            INotificationChannel whatsappChannel = new WhatsappChannel();
            INotificationChannel discordChannel = new DiscordChannel();

            List<INotificationChannel> channels = new List<INotificationChannel>
            {
                emailChannel,
                whatsappChannel,
                discordChannel
            };

            NotificationService notificationService = new NotificationService(channels);
            notificationService.NotifyAll("Hello, this is a test notification!", "John Doe");
        }
    }
}