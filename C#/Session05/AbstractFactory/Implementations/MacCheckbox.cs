using AbstractFactory.Interfaces;

namespace AbstractFactory.Implementations
{
    public class MacCheckbox : ICheckbox
    {
        public void Render()
        {
            Console.WriteLine("Rendering macOS Checkbox");
        }
    }
}