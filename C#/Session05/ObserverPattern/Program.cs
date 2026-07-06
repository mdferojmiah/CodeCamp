using ObserverPattern.Implementations;
using ObserverPattern.Interfaces;

namespace ObserverPattern
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----Observer Pattern-----");
            var agency = new NewsAgency();

            IObserver channel1 = new NewsChannel();
            IObserver channel2 = new NewsChannel();
            IObserver channel3 = new NewsChannel();

            agency.Subscribe(channel1);
            agency.Subscribe(channel2);
            agency.Subscribe(channel3);

            agency.NotifyAll("Iran nuke the USA!");

            agency.Unsubscribe(channel2);

            agency.NotifyAll("GTA VI has been released!");
        }
    }
}