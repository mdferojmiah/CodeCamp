using DecoratorPattern.Interfaces;

namespace DecoratorPattern.Classes
{
    public class SugerDecorator: CoffeeDecorator
    {
        public SugerDecorator(ICoffee coffee): base(coffee){}

        public override string GetDescription()
        {
            return _coffee.GetDescription() + " with extra Suger.";
        }

        public override double GetPrice()
        {
            return _coffee.GetPrice() + 2.0;
        }
    }
}