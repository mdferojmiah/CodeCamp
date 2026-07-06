using AbstractFactory.Interfaces;

namespace AbstractFactory.Implementations
{
    public class WinBtn : IButton
    {
        public void Render()
        {
            Console.WriteLine("Rendering Windows Button");
        }
    }
}