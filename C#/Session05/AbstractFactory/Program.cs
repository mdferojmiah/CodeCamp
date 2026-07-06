using AbstractFactory.Factories;
using AbstractFactory.Interfaces;

namespace AbstractFactory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("------Abstract Factory Pattern------");

            IGUIFactory winFactory = new WinFactory();
            IButton winBtn = winFactory.CreateButton();
            winBtn.Render();

            ICheckbox winCheckbox = winFactory.CreateCheckbox();
            winCheckbox.Render();

            IGUIFactory macFactory = new MacFactory();
            IButton macBtn = macFactory.CreateButton();
            macBtn.Render();

            ICheckbox macCheckbox = macFactory.CreateCheckbox();
            macCheckbox.Render();
        }
    }
}