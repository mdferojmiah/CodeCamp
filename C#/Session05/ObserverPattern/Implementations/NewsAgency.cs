using ObserverPattern.Interfaces;

namespace ObserverPattern.Implementations
{
    public class NewsAgency
    {
        private readonly List<IObserver> _observers = new();

        public void NotifyAll(string news)
        {
            foreach(var observer in _observers)
            {
                observer.Notify(news);
            }
        }

        public void Subscribe(IObserver observer) => _observers.Add(observer);

        public void Unsubscribe(IObserver observer) => _observers.Remove(observer);
    }
}