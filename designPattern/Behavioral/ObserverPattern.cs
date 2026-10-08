namespace designPattern.Behavioral;

// 1. Observer Interface (Subscriber)
public interface IStockObserver
{
    void OnPriceChanged(string stockSymbol, decimal newPrice);
}

// 2. Subject / Publisher (Stock Ticker)
public class StockTicker
{
    private readonly string _symbol;
    private decimal _price;
    private readonly List<IStockObserver> _observers = new();

    public StockTicker(string symbol, decimal initialPrice)
    {
        _symbol = symbol;
        _price = initialPrice;
    }

    public void Attach(IStockObserver observer)
    {
        _observers.Add(observer);
        Console.WriteLine($"[Ticker] Added a new subscriber to {_symbol}.");
    }

    public void Detach(IStockObserver observer)
    {
        _observers.Remove(observer);
        Console.WriteLine($"[Ticker] Removed a subscriber from {_symbol}.");
    }

    public void SetPrice(decimal newPrice)
    {
        if (_price != newPrice)
        {
            _price = newPrice;
            Console.WriteLine($"\n--- [MARKET UPDATE] {_symbol} moved to ${_price} ---");
            Notify();
        }
    }

    private void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.OnPriceChanged(_symbol, _price);
        }
    }
}

// 3. Concrete Observers
public class MobileAppAlert : IStockObserver
{
    public void OnPriceChanged(string stockSymbol, decimal newPrice)
    {
        Console.WriteLine($"📱 [Mobile App] Push Alert: {stockSymbol} is now ${newPrice}!");
    }
}

public class AutoTradingBot : IStockObserver
{
    private readonly decimal _buyThreshold;

    public AutoTradingBot(decimal buyThreshold)
    {
        _buyThreshold = buyThreshold;
    }

    public void OnPriceChanged(string stockSymbol, decimal newPrice)
    {
        if (newPrice < _buyThreshold)
        {
            Console.WriteLine($"🤖 [Trading Bot] BUY ORDER EXECUTED! Price ${newPrice} is below target ${_buyThreshold}.");
        }
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class ObserverExample
{
    public static void Run()
    {
        Console.WriteLine("=== OBSERVER PATTERN ===");

        var btcTicker = new StockTicker("BTC", 65000m);

        var mobileApp = new MobileAppAlert();
        var tradingBot = new AutoTradingBot(buyThreshold: 60000m);

        // 1. Subscribe
        btcTicker.Attach(mobileApp);
        btcTicker.Attach(tradingBot);

        // 2. Price changes -> both notified
        btcTicker.SetPrice(62000m);

        // 3. Price drops below threshold -> bot executes buy order
        btcTicker.SetPrice(59000m);

        // 4. Mobile app unsubscribes
        btcTicker.Detach(mobileApp);

        // 5. Price changes again -> only bot receives update
        btcTicker.SetPrice(58000m);

        Console.WriteLine();
    }
}
