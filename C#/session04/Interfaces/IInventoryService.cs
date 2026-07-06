using session04.Domain;

namespace session04.Interfaces
{
    public interface IInventoryService
    {
        bool IsInStock(Order order);
    }
}