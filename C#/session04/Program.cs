using session04.Domain;
using session04.Interfaces;
using session04.Services;

namespace session04
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Order order1 = new Order
            {
                Id = 1,
                Item = "Pen",
                Amount = 20.0m
            };

            Order order2 = new Order
            {
                Id = 2,
                Item = "GTA VI",
                Amount = 20000.0m
            };

            IOrderValidator orderValidator = new OrderValidator();
            IInventoryService inventoryService = new InventoryService();
            IPaymentProcessor paymentProcessor = new PaymentProcessor();
            INotificationService notificationService = new PushNotificationService();

            OrderProcessor orderProcessor = new OrderProcessor(orderValidator, inventoryService, notificationService, paymentProcessor);
            orderProcessor.ProcessOrder(order1);
            Console.WriteLine();
            orderProcessor.ProcessOrder(order2);
        }
    }
}