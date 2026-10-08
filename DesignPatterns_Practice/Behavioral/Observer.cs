
namespace DesignPatterns_Practice.Behavioral;

public interface IStockObserver
{
    void OnPriceChanged(string stockSymbol, decimal newPrice);
}
public class StockTicker
{
    private readonly string symbol;
    private decimal _price;

    private readonly List<IStockObserver> _observers = new List<IStockObserver>();

    public StockTicker(string symbol, decimal initialPrice)
    {
        this.symbol = symbol;
        this._price = initialPrice;
    }

    public void Attach(IStockObserver observer)
    {
        _observers.Add(observer);
        Console.WriteLine($"[Ticker] Added a new subscriber to {symbol}");
    }
    public void Detach(IStockObserver observer)
    {
        _observers.Remove(observer);
        Console.WriteLine($"[Ticker] Removed a new subscriber to {symbol}");
    }

    public void SetPrice(decimal newPrice)
    {
        if (newPrice != _price)
        {
            _price = newPrice;
            NotifyObservers();
        }
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.OnPriceChanged(symbol, _price);
        }
    }

}

// Subscriber 1: Mobile App
public class MobileAppAlert : IStockObserver
{
    public void OnPriceChanged(string stockSymbol, decimal newPrice)
    {
        Console.WriteLine($"📱 [Mobile App] Push Alert: {stockSymbol} is now ${newPrice}!");
    }
}

// Subscriber 2: Automated Trading Bot
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


public class ObserverExample
{
    public static void Run()
    {
        var btcTicker = new StockTicker("BTC", 65000m);

        var mobileApp = new MobileAppAlert();
        var tradingBot = new AutoTradingBot(buyThreshold: 60000m);

        // 1. Subscribe both services
        btcTicker.Attach(mobileApp);
        btcTicker.Attach(tradingBot);

        // 2. Price changes -> Both are notified
        btcTicker.SetPrice(62000m);

        // 3. Price drops below bot threshold -> Bot executes buy order!
        btcTicker.SetPrice(59000m);

        // 4. Mobile app unsubscribes
        btcTicker.Detach(mobileApp);

        // 5. Price changes again -> ONLY the bot is notified!
        btcTicker.SetPrice(58000m);
    }
}
