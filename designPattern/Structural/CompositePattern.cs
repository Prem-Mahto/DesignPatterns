namespace designPattern.Structural;

// 1. Common Component Interface
public interface ICatalogItem
{
    string Name { get; }
    decimal GetPrice();
    void Display(int indent = 0);
}

// 2. Leaf (Individual Product)
public class Product : ICatalogItem
{
    public string Name { get; }
    private readonly decimal _price;

    public Product(string name, decimal price)
    {
        Name = name;
        _price = price;
    }

    public decimal GetPrice() => _price;

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent * 2)}- {Name} (${_price})");
    }
}

// 3. Composite (Bundle of items / sub-bundles)
public class ProductBundle : ICatalogItem
{
    public string Name { get; }
    private readonly List<ICatalogItem> _items = new();
    private readonly decimal _discountPercentage;

    public ProductBundle(string name, decimal discountPercentage = 0)
    {
        Name = name;
        _discountPercentage = discountPercentage;
    }

    public void Add(ICatalogItem item) => _items.Add(item);
    public void Remove(ICatalogItem item) => _items.Remove(item);

    public decimal GetPrice()
    {
        decimal subTotal = _items.Sum(item => item.GetPrice());
        decimal discount = subTotal * (_discountPercentage / 100m);
        return subTotal - discount;
    }

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent * 2)}+ [BUNDLE] {Name} (Discount: {_discountPercentage}%)");
        foreach (var item in _items)
        {
            item.Display(indent + 1); // Recursive call
        }
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class CompositeExample
{
    public static void Run()
    {
        Console.WriteLine("=== COMPOSITE PATTERN ===");

        // Leaves
        var mouse = new Product("Gaming Mouse", 25m);
        var keyboard = new Product("Mechanical Keyboard", 75m);
        var monitor = new Product("4K Monitor", 400m);

        // Child bundle (10% discount)
        var gamerPack = new ProductBundle("Gamer Accessories Pack", discountPercentage: 10);
        gamerPack.Add(mouse);
        gamerPack.Add(keyboard);

        // Root bundle (5% bundle discount)
        var ultimateSetup = new ProductBundle("Ultimate Office Setup", discountPercentage: 5);
        ultimateSetup.Add(monitor);
        ultimateSetup.Add(gamerPack); // Adding bundle inside bundle!

        // Uniform consumption
        ultimateSetup.Display();
        Console.WriteLine($"\nFinal Calculated Total: ${ultimateSetup.GetPrice():F2}");
        Console.WriteLine();
    }
}
