namespace designPattern.Creational;

// 1. The Common Product Interface
public interface IPaymentProcessor
{
    string MethodName { get; }
    void Charge(decimal amount);
}

// Concrete Product 1
public class CreditCardProcessor : IPaymentProcessor
{
    public string MethodName => "CreditCard";
    public void Charge(decimal amount) =>
        Console.WriteLine($"[CreditCard] Charged ${amount} via Card Gateway.");
}

// Concrete Product 2
public class PayPalProcessor : IPaymentProcessor
{
    public string MethodName => "PayPal";
    public void Charge(decimal amount) =>
        Console.WriteLine($"[PayPal] Redirecting to PayPal to authorize ${amount}.");
}

// Concrete Product 3
public class ApplePayProcessor : IPaymentProcessor
{
    public string MethodName => "ApplePay";
    public void Charge(decimal amount) =>
        Console.WriteLine($"[ApplePay] Charged ${amount} via Apple Pay biometric token.");
}

// -------------------------------------------------------------
// 2. The Classic GoF Factory Method (Subclasses decide instantiation)
// -------------------------------------------------------------
public abstract class PaymentProcessorFactory
{
    public abstract IPaymentProcessor CreateProcessor();

    public void ExecutePayment(decimal amount)
    {
        IPaymentProcessor processor = CreateProcessor();
        Console.WriteLine("--> [Factory Method] Applying fraud checks...");
        processor.Charge(amount);
        Console.WriteLine("--> [Factory Method] Audit log saved.");
    }
}

public class CreditCardFactory : PaymentProcessorFactory
{
    public override IPaymentProcessor CreateProcessor() => new CreditCardProcessor();
}

public class PayPalFactory : PaymentProcessorFactory
{
    public override IPaymentProcessor CreateProcessor() => new PayPalProcessor();
}

public class ApplePayFactory : PaymentProcessorFactory
{
    public override IPaymentProcessor CreateProcessor() => new ApplePayProcessor();
}

// -------------------------------------------------------------
// 3. Modern .NET Open/Closed Resolver Pattern
// -------------------------------------------------------------
public class ModernPaymentFactory
{
    private readonly Dictionary<string, IPaymentProcessor> _processors;

    public ModernPaymentFactory(IEnumerable<IPaymentProcessor> processors)
    {
        _processors = processors.ToDictionary(
            p => p.MethodName,
            p => p,
            StringComparer.OrdinalIgnoreCase
        );
    }

    public IPaymentProcessor GetProcessor(string method)
    {
        if (_processors.TryGetValue(method, out var processor))
        {
            return processor;
        }

        throw new NotSupportedException($"Payment method '{method}' is not supported.");
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class FactoryExample
{
    public static void Run()
    {
        Console.WriteLine("=== FACTORY PATTERN ===");

        // Part 1: Classic GoF Factory Method
        Console.WriteLine("--- 1. GoF Factory Method (Subclasses) ---");
        PaymentProcessorFactory cardFactory = new CreditCardFactory();
        cardFactory.ExecutePayment(150m);

        PaymentProcessorFactory payPalFactory = new PayPalFactory();
        payPalFactory.ExecutePayment(75m);

        // Part 2: Modern .NET Resolver Pattern (Dictionary / DI)
        Console.WriteLine("\n--- 2. Modern .NET Resolver Factory ---");
        var availableProcessors = new IPaymentProcessor[]
        {
            new CreditCardProcessor(),
            new PayPalProcessor(),
            new ApplePayProcessor()
        };

        var modernFactory = new ModernPaymentFactory(availableProcessors);
        var applePay = modernFactory.GetProcessor("ApplePay");
        applePay.Charge(299m);

        Console.WriteLine();
    }
}
