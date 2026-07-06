using session04.Domain;
using session04.Interfaces;

namespace session04.Services
{
    public class InventoryService : IInventoryService
    {
        public bool IsInStock(Order order)
        {
            if(order.Id  % 2 == 0)
            {
                Console.WriteLine($"Order {order.Id} is in stock.");
                return true;
            }
            else
            {
                Console.WriteLine($"Order {order.Id} is not in stock.");
                return false;
            }
        }
    }
}