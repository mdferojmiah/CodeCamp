using AbstractFactory.Implementations;
using AbstractFactory.Interfaces;

namespace AbstractFactory.Factories
{
    public class MacFactory : IGUIFactory
    {
        public IButton CreateButton()
        {
            return new MacBtn();
        }

        public ICheckbox CreateCheckbox()
        {
            return new MacCheckbox();
        }
    }
}