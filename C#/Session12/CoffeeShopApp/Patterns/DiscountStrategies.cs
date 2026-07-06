namespace CoffeeShopApp.Patterns;

public interface IDiscountStrategy
{
    decimal GetDiscountedAmount(decimal amount);
}

public class HappyCoustomerDiscount : IDiscountStrategy
{
    public decimal GetDiscountedAmount(decimal amount) => amount - amount * 0.10m;
}

public class LuckyCustomerDiscount : IDiscountStrategy
{
    public decimal GetDiscountedAmount(decimal amount) => amount - amount * 0.25m;
}

public class RegularDiscount : IDiscountStrategy
{
    public decimal GetDiscountedAmount(decimal amount) => amount - amount * 0.05m;
}