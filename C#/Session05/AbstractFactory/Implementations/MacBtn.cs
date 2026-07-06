using AbstractFactory.Interfaces;

namespace AbstractFactory.Implementations
{
    public class MacBtn : IButton
    {
        public void Render()
        {
            Console.WriteLine("Rendering macOS Button");
        }
    }
}