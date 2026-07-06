using session04.Domain;
using session04.Interfaces;

namespace session04.Services
{
    public class PaymentProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(Order order)
        {
            Console.WriteLine($"Processing payment...\nOrder ID: {order.Id}\nItem: {order.Item}\nAmount: {order.Amount}");

            Console.WriteLine($"Payment for Order {order.Id} processed successfully.");
            return true;
        }
    }
}