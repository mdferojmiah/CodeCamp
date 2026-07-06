using DecoratorPattern.Interfaces;

namespace DecoratorPattern.Classes
{
    public class SimpleCoffee : ICoffee
    {
        public string GetDescription() => $"Simple Coffee";

        public double GetPrice() => 10;
    }
}