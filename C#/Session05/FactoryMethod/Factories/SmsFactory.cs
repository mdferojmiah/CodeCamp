using FactoryMethod.Implementations;
using FactoryMethod.Interfaces;

namespace FactoryMethod.Factories
{
    public class SmsFactory: NotificationFactory
    {
        public override INotification CreateNotification()
        {
            return new SmsNotification();
        }
    }
}