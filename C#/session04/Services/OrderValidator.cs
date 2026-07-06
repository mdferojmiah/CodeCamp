using session04.Domain;
using session04.Interfaces;

namespace session04.Services
{
    public class OrderValidator : IOrderValidator
    {
        public bool Validate(Order order)
        {
            if(order.Id < 0  || string.IsNullOrEmpty(order.Item) || order.Amount <= 0)
            {
                Console.WriteLine($"Order {order.Id} is invalid.");
                return false;
            }
            Console.WriteLine($"Order {order.Id} is valid.");
            return true;
        }
    }
}