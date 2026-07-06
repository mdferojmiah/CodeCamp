namespace EventPractice
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello Event");
            // TemperatureSensor sensor = new TemperatureSensor();
            // sensor.TemperatureChanged += AlertTemperatureHandler;
            // sensor.Temperature = 35.6;
            // sensor.Temperature = 41.3;

            var publisher = new NewsPublisher();
            // publisher.NewsPublished += msg => Console.WriteLine($"Email sent: {msg}");
            // publisher.NewsPublished += msg => Console.WriteLine($"SMS sent: {msg}");
            publisher.NewsPublished += EmailChannel;
            publisher.NewsPublished += SMSChannel;

            publisher.Publish("A news on BNP leader collecting chanda!");
        }

        static void AlertTemperatureHandler(double temp)
        {
            Console.WriteLine($"Alert: {temp} degree C");
        }

        static void EmailChannel(string msg)
        {
            Console.WriteLine($"Email sent: {msg}");
        }

        static void SMSChannel(string msg)
        {
            Console.WriteLine($"SMS sent: {msg}");
        }
    }
}