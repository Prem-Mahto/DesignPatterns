using System;

namespace DesignPatterns_Practice.Behavioral;

// ❌ Disaster: A single class trying to know every single math formula in the company
public class OrderService
{
    public decimal CalculateFinalPrice(decimal finalPrice, string discountType)
    {
        switch (discountType)
        {
            case "None":
                break;

            case "Vip":
                finalPrice -= finalPrice * 0.20m;
                break;

            case "BlackFriday":
                if (finalPrice > 100)
                    finalPrice -= 25m;
                break;

            case "FirstTimeBuyer":
                finalPrice -= finalPrice * 0.10m;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return finalPrice;
    }
}

public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal cartSubTotal);
}

public class NoDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal cartSubTotal)=> cartSubTotal;
}

public class TwentyPercentDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal cartSubTotal) => cartSubTotal * 0.80m;
}

public class CheckOutService
{
    private readonly IDiscountStrategy discountStrategy;

    public CheckOutService( IDiscountStrategy discountStrategy)
    {
        this.discountStrategy = discountStrategy;
    }

    public decimal CheckOut(decimal cartSubtotal)
    {
        Console.WriteLine($"Cart subtotal: {cartSubtotal}");
        var finalPrice= discountStrategy.ApplyDiscount(cartSubtotal);
        Console.WriteLine($"Cart final price: {finalPrice}");
        return finalPrice;
    }
}

public class StrategyExample
{
 public static void Run()
    {
     new CheckOutService (new NoDiscount()).CheckOut(100);
     new CheckOutService (new TwentyPercentDiscount()).CheckOut(100);

    }
}
