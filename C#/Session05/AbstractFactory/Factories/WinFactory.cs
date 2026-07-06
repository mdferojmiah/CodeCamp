using AbstractFactory.Implementations;
using AbstractFactory.Interfaces;

namespace AbstractFactory.Factories
{
    public class WinFactory : IGUIFactory
    {
        public IButton CreateButton()
        {
            return new WinBtn();
        }

        public ICheckbox CreateCheckbox()
        {
            return new WinCheckbox();
        }
    }
}