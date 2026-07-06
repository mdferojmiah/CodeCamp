using AdapterPattern.Interfaces;

namespace AdapterPattern.Classes
{
    public class Laptop
    {
        public void Charge(IUSASocket socket)
        {
            Console.WriteLine($"Charging using USA Socket {socket.GetUSAScoket()}");
        }
    }
}