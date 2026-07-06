using session04.Domain;

namespace session04.Interfaces
{
    public interface IOrderValidator
    {
        bool Validate(Order order);
    }
}