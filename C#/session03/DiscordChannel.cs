namespace session03
{
    public class DiscordChannel: INotificationChannel
    {
        public void SendNotification(string message, string recipient)
        {
            Console.WriteLine($"Sending Discord message to {recipient}: {message}");
        }
    }
}