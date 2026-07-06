namespace StrategyPattern.Interfaces
{
    public interface IPaymentStrategy
    {
        void pay(decimal amount);
    }
}