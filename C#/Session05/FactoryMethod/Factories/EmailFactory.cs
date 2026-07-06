using FactoryMethod.Implementations;
using FactoryMethod.Interfaces;

namespace FactoryMethod.Factories
{
    public class EmailFactory : NotificationFactory
    {
        public override INotification CreateNotification()
        {
            return new EmailNotification();
        }
    }
}