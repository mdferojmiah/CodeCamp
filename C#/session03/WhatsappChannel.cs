namespace session03
{
    public class WhatsappChannel: INotificationChannel
    {
        public void SendNotification(string message, string recipient)
        {
            Console.WriteLine($"Sending WhatsApp message to {recipient}: {message}");
        }
    }
}