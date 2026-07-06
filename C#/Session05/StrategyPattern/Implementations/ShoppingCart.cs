using StrategyPattern.Interfaces;

namespace StrategyPattern.Implementations
{
    public class ShoppingCart
    {
        private IPaymentStrategy? _strategy;

        public void SetStrategy(IPaymentStrategy strategy) => _strategy = strategy;

        public void Checkout(decimal amount)
        {
            _strategy!.pay(amount);
        }
    }   
}