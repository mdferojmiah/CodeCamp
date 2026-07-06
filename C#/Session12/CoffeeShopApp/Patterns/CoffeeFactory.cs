using CoffeeShopApp.Models;

namespace CoffeeShopApp.Patterns;

public static class CoffeeFactory
{
    public static ICoffee CreateCoffee(string type)
    {
        return type.ToLower() switch
        {
            "espresso" => new Espresso(),
            "latte" => new Latte(),
            _ => new RegularCoffee()
        };
    }
}