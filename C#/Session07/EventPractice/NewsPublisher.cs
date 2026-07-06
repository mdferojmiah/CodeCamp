namespace EventPractice
{
    public class NewsPublisher
    {
        public event Action<string>? NewsPublished;

        public void Publish(string news)
        {
            Console.WriteLine($"Publishing: {news}");
            NewsPublished?.Invoke(news);
        }
    }
}