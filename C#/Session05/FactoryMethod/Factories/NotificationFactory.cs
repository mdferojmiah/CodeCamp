using FactoryMethod.Interfaces;

namespace FactoryMethod.Factories
{
    public abstract class NotificationFactory
    {
        public abstract INotification CreateNotification();
    }
}