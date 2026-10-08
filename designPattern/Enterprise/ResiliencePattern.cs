namespace designPattern.Enterprise;

public enum CircuitState
{
    Closed,   // Normal operation, requests pass through
    Open,     // Failure threshold reached, fail fast without calling downstream
    HalfOpen  // Probe trial to test if downstream recovered
}

// 1. Circuit Breaker Engine
public class CircuitBreaker
{
    private readonly int _failureThreshold;
    private readonly TimeSpan _openToHalfOpenWaitTime;

    private int _consecutiveFailures = 0;
    private DateTime _lastStateChangeUtc = DateTime.UtcNow;

    public CircuitState State { get; private set; } = CircuitState.Closed;

    public CircuitBreaker(int failureThreshold, TimeSpan waitTime)
    {
        _failureThreshold = failureThreshold;
        _openToHalfOpenWaitTime = waitTime;
    }

    public T Execute<T>(Func<T> action)
    {
        // Check if Open circuit is ready to transition to Half-Open trial
        if (State == CircuitState.Open)
        {
            if (DateTime.UtcNow - _lastStateChangeUtc > _openToHalfOpenWaitTime)
            {
                State = CircuitState.HalfOpen;
                _lastStateChangeUtc = DateTime.UtcNow;
                Console.WriteLine("  🟡 [CIRCUIT BREAKER] Transitioned to HALF-OPEN (Testing probe request)...");
            }
            else
            {
                Console.WriteLine("  🔴 [CIRCUIT BREAKER] Circuit is OPEN! Failing fast without calling downstream API.");
                throw new InvalidOperationException("CircuitBreakerOpenException: Downstream service is currently unavailable.");
            }
        }

        try
        {
            T result = action();

            // Success resets circuit
            if (State == CircuitState.HalfOpen)
            {
                State = CircuitState.Closed;
                _consecutiveFailures = 0;
                Console.WriteLine("  🟢 [CIRCUIT BREAKER] Probe request succeeded! Circuit reset to CLOSED.");
            }
            return result;
        }
        catch (Exception)
        {
            _consecutiveFailures++;
            Console.WriteLine($"  ⚠️ [FAILURE] Request failed. Consecutive failures: {_consecutiveFailures}/{_failureThreshold}");

            if (_consecutiveFailures >= _failureThreshold || State == CircuitState.HalfOpen)
            {
                State = CircuitState.Open;
                _lastStateChangeUtc = DateTime.UtcNow;
                Console.WriteLine("  🔴 [CIRCUIT BREAKER] Failure threshold reached! Circuit tripped to OPEN.");
            }
            throw;
        }
    }
}

// 2. Retry Policy with Exponential Backoff
public class RetryPolicy
{
    public static T ExecuteWithRetry<T>(Func<T> operation, int maxAttempts = 3, int delayMs = 100)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return operation();
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                Console.WriteLine($"  [RETRY] Attempt {attempt}/{maxAttempts} failed: {ex.Message}. Retrying in {delayMs}ms...");
                Thread.Sleep(delayMs);
                delayMs *= 2; // Exponential backoff
            }
        }
        return operation(); // Last attempt will throw if it fails
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class ResilienceExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- .NET Enterprise Pattern 6: Resilience Patterns (Circuit Breaker & Retry) ---");

        var breaker = new CircuitBreaker(failureThreshold: 2, waitTime: TimeSpan.FromMilliseconds(500));
        int callCount = 0;

        // Simulated unreliable downstream microservice
        string UnreliableApi()
        {
            callCount++;
            if (callCount <= 2)
            {
                throw new HttpRequestException("503 Service Unavailable");
            }
            return "SUCCESS_DATA_200_OK";
        }

        Console.WriteLine("\n1. Simulating failures tripping the Circuit Breaker:");
        for (int i = 1; i <= 3; i++)
        {
            try
            {
                Console.WriteLine($"\nClient Request #{i}:");
                breaker.Execute(UnreliableApi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client caught: {ex.GetType().Name}");
            }
        }

        Console.WriteLine("\n2. Waiting for cool-off period to trigger Half-Open probe...");
        Thread.Sleep(600);

        try
        {
            Console.WriteLine("\nClient Request #4 (After cool-off):");
            string data = breaker.Execute(UnreliableApi);
            Console.WriteLine($"Received Response: {data}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client caught: {ex.Message}");
        }
    }
}
