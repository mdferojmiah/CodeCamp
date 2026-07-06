using DecoratorPattern.Interfaces;

namespace DecoratorPattern.Classes
{
    public abstract class CoffeeDecorator: ICoffee
    {
        protected ICoffee _coffee;

        protected CoffeeDecorator(ICoffee coffee)
        {
            _coffee = coffee;
        }

        public virtual string GetDescription() => _coffee.GetDescription();

        public virtual double GetPrice() => _coffee.GetPrice();
    }
}