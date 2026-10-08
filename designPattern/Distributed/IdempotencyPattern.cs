namespace designPattern.Distributed;

// 1. Models
public record PaymentRequest(decimal Amount, string CardToken);
public record PaymentResponse(Guid TransactionId, decimal Amount, string Status, DateTime ProcessedAt);

// 2. Simulated Distributed Cache (Redis)
public class MockDistributedCache
{
    private readonly Dictionary<string, (PaymentResponse Response, DateTime ExpiresAt)> _store = new();

    public PaymentResponse? Get(string key)
    {
        if (_store.TryGetValue(key, out var entry))
        {
            if (DateTime.UtcNow <= entry.ExpiresAt)
                return entry.Response;

            _store.Remove(key); // Expired
        }
        return null;
    }

    public void Set(string key, PaymentResponse response, TimeSpan ttl)
    {
        _store[key] = (response, DateTime.UtcNow.Add(ttl));
    }
}

// 3. Payment Gateway Controller with Idempotency Protection
public class IdempotentPaymentGateway
{
    private readonly MockDistributedCache _cache;
    private int _actualBankTransactionsCount = 0;

    public int ActualBankTransactionsCount => _actualBankTransactionsCount;

    public IdempotentPaymentGateway(MockDistributedCache cache) => _cache = cache;

    public PaymentResponse ProcessPayment(Guid idempotencyKey, PaymentRequest request)
    {
        string cacheKey = $"idempotency:pay:{idempotencyKey}";

        // Step 1: Check if this idempotency key was already processed
        var cached = _cache.Get(cacheKey);
        if (cached != null)
        {
            Console.WriteLine($"  ⚡ [CACHE-HIT] Request '{idempotencyKey.ToString()[..8]}' was already processed! Returning cached response (Zero extra bank charge).");
            return cached;
        }

        // Step 2: First-time processing (Actual mutation)
        _actualBankTransactionsCount++;
        Console.WriteLine($"  💳 [BANK-TRANSACTION #{_actualBankTransactionsCount}] Charging credit card ${request.Amount} for key: '{idempotencyKey.ToString()[..8]}'");

        var response = new PaymentResponse(
            TransactionId: Guid.NewGuid(),
            Amount: request.Amount,
            Status: "Charged_Successfully",
            ProcessedAt: DateTime.UtcNow
        );

        // Step 3: Cache result for 24 hours
        _cache.Set(cacheKey, response, TimeSpan.FromHours(24));

        return response;
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class IdempotencyExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- Tier 2 Pattern 2: The Idempotency Pattern ---");

        var cache = new MockDistributedCache();
        var gateway = new IdempotentPaymentGateway(cache);

        var userKey = Guid.NewGuid();
        var request = new PaymentRequest(100.00m, "tok_visa_4242");

        Console.WriteLine($"Simulating mobile checkout with Client Idempotency-Key: {userKey.ToString()[..8]}\n");

        // Attempt 1: First click
        Console.WriteLine("1. First Request sent by user:");
        var res1 = gateway.ProcessPayment(userKey, request);
        Console.WriteLine($"   Status: {res1.Status}, TxId: {res1.TransactionId.ToString()[..8]}");

        // Attempt 2: Network retry with the SAME key (e.g. timeout on phone)
        Console.WriteLine("\n2. Network timeout auto-retry sent by mobile phone (Same Idempotency Key):");
        var res2 = gateway.ProcessPayment(userKey, request);
        Console.WriteLine($"   Status: {res2.Status}, TxId: {res2.TransactionId.ToString()[..8]}");

        // Attempt 3: Another retry
        Console.WriteLine("\n3. Third duplicate retry arrives:");
        var res3 = gateway.ProcessPayment(userKey, request);

        Console.WriteLine($"\nVerification: Total bank charges executed: {gateway.ActualBankTransactionsCount} (Customer was charged exactly once!)");
    }
}
