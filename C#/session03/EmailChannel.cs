namespace session03
{
    public class EmailChannel: INotificationChannel
    {
        public void SendNotification(string message, string recipient)
        {
            Console.WriteLine($"Sending email to {recipient}: {message}");
        }
    }
}