namespace designPattern.Behavioral;

// 1. The Strategy Interface
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal totalAmount);
}

// 2. Concrete Strategies
public class NoDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal totalAmount) => totalAmount;
}

public class VipDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal totalAmount) => totalAmount * 0.80m;
}

public class BlackFridayDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal totalAmount)
    {
        if (totalAmount > 100m)
            return totalAmount - 25m;

        return totalAmount;
    }
}

// 3. The Context (Consumes Strategy)
public class CheckoutService
{
    private IDiscountStrategy _discountStrategy;

    public CheckoutService(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public void SetStrategy(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal Checkout(decimal cartTotal)
    {
        Console.WriteLine($"Original Total: ${cartTotal}");
        decimal finalAmount = _discountStrategy.ApplyDiscount(cartTotal);
        Console.WriteLine($"Final Amount after discount: ${finalAmount}");
        return finalAmount;
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class StrategyExample
{
    public static void Run()
    {
        Console.WriteLine("=== STRATEGY PATTERN ===");

        decimal cartTotal = 150m;

        // 1. Regular checkout with No Discount
        Console.WriteLine("--- 1. No Discount Strategy ---");
        var checkout = new CheckoutService(new NoDiscountStrategy());
        checkout.Checkout(cartTotal);

        // 2. VIP Member logs in -> swap strategy at runtime!
        Console.WriteLine("\n--- 2. VIP Discount Strategy (20% off) ---");
        checkout.SetStrategy(new VipDiscountStrategy());
        checkout.Checkout(cartTotal);

        // 3. Black Friday sale -> swap strategy!
        Console.WriteLine("\n--- 3. Black Friday Strategy ($25 off over $100) ---");
        checkout.SetStrategy(new BlackFridayDiscountStrategy());
        checkout.Checkout(cartTotal);

        Console.WriteLine();
    }
}
