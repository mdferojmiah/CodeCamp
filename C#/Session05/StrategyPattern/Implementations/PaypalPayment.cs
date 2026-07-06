using StrategyPattern.Interfaces;

namespace StrategyPattern.Implementations
{
    public class PaypalPayment : IPaymentStrategy
    {
        public void pay(decimal amount) => Console.WriteLine($"Paid {amount} via Paypal.");
    }
}