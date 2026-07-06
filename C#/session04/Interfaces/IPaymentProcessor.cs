using session04.Domain;

namespace session04.Interfaces
{
    public interface IPaymentProcessor
    {
        bool ProcessPayment(Order order);
    }
}