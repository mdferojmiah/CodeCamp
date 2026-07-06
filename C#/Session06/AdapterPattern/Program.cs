using AdapterPattern.Classes;

namespace AdapterPattern
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----Adapter Pattern-----");
            EuropeanPlug europeanPlug = new EuropeanPlug();
            Laptop laptop = new Laptop();

            SocketAdapter socketAdapter = new SocketAdapter(europeanPlug);
            laptop.Charge(socketAdapter);
        }
    }
}