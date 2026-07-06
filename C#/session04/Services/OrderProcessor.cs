using session04.Domain;
using session04.Interfaces;

namespace session04.Services
{
    public class OrderProcessor
    {
        private readonly IOrderValidator _orderValidator;
        private readonly IInventoryService _inventoryService;
        private readonly INotificationService _notificationService;
        private readonly IPaymentProcessor _paymentProcessor;

        public OrderProcessor(
            IOrderValidator orderValidator,
            IInventoryService inventoryService,
            INotificationService notificationService,
            IPaymentProcessor paymentProcessor)
        {
            _orderValidator = orderValidator;
            _inventoryService = inventoryService;
            _notificationService = notificationService;
            _paymentProcessor = paymentProcessor;
        }


        public void ProcessOrder(Order order)
        {
            var validationResult = _orderValidator.Validate(order);
            if (!validationResult)
            {
                Console.WriteLine($"[Validation Failed]: Order validation failed!");
                return;
            }

            var isProductInStock = _inventoryService.IsInStock(order);

            if (!isProductInStock)
            {
                Console.WriteLine($"[Inventory Failed]: Product is not in storck");
                return;
            }

            var isPaymentProcessed = _paymentProcessor.ProcessPayment(order);

            if (!isPaymentProcessed)
            {
                Console.WriteLine($"[Payment Failed]: Payment couldn't be processed!");
                return;
            }

            _notificationService.SendNotification(order);
            Console.WriteLine($"[Success]: Product purchase done!");
        }
    }
}