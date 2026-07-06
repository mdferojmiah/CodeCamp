namespace CoffeeShopApp.Models;

public class Espresso : ICoffee
{
    public decimal GetCost() => 40.00m;

    public string GetDescription() => "Espresso";
}

public class Latte : ICoffee
{
    public decimal GetCost() => 60.00m;

    public string GetDescription() => "Latte";
}

public class RegularCoffee : ICoffee
{
    public decimal GetCost() => 30.00m;

    public string GetDescription() => "Regular";
}