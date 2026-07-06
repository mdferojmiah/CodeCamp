using StrategyPattern.Implementations;

namespace StrategyPattern
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----Strategy Pattern-----");
            ShoppingCart cart = new ShoppingCart();
            cart.SetStrategy(new CreditCardPayment());
            cart.Checkout(200);

            cart.SetStrategy(new PaypalPayment());
            cart.Checkout(500);
        }
    }
}