using DecoratorPattern.Classes;
using DecoratorPattern.Interfaces;

namespace DecoratorPattern
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----Decorator Pattern-----");
            ICoffee coffee = new SimpleCoffee();
            Console.WriteLine($"Description: {coffee.GetDescription()}\nPrice: {coffee.GetPrice()}$");

            //coffee = new MilkDecorator(coffee);
            //Console.WriteLine($"Description: {coffee.GetDescription()}\nPrice: {coffee.GetPrice()}$");

            coffee = new SugerDecorator(coffee);
            Console.WriteLine($"Description: {coffee.GetDescription()}\nPrice: {coffee.GetPrice()}$");
        }
    }
}