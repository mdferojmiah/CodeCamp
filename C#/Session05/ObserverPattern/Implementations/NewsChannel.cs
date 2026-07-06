using ObserverPattern.Interfaces;

namespace ObserverPattern.Implementations
{
    public class NewsChannel : IObserver
    {
        public void Notify(string message) => Console.WriteLine($"Breaking News: {message}");
    }
}