using AbstractFactory.Interfaces;

namespace AbstractFactory.Implementations
{
    public class WinCheckbox : ICheckbox
    {
        public void Render()
        {
            Console.WriteLine("Rendering Windows Checkbox");
        }
    }
}