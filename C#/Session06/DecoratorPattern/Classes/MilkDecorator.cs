using DecoratorPattern.Interfaces;

namespace DecoratorPattern.Classes
{
    public class MilkDecorator: CoffeeDecorator
    {
        public MilkDecorator(ICoffee coffee): base(coffee) {}

        public override string GetDescription() => _coffee.GetDescription() + " with Milk.";
        public override double GetPrice() => _coffee.GetPrice() + 5.0;
    }
}