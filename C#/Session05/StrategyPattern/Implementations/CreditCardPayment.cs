using StrategyPattern.Interfaces;

namespace StrategyPattern.Implementations
{
    public class CreditCardPayment : IPaymentStrategy
    {
        public void pay(decimal amount) => Console.WriteLine($"Paid {amount} via CreditCard.");
    }
}