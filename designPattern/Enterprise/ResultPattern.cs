namespace designPattern.Enterprise;

// 1. Error Representation
public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}

// 2. The Generic Result Type (Railway-Oriented Programming)
public class Result<TValue>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public TValue? Value { get; }
    public Error Error { get; }

    private Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = Error.None;
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(Error error) => new(error);

    // Implicit conversions for clean syntax
    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);

    // Functional Railway Matching
    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<Error, TResult> onFailure)
        => IsSuccess ? onSuccess(Value!) : onFailure(Error);
}

// 3. Domain Service using Result Pattern (Zero Exceptions for Expected Failures)
public class BankAccount
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public Result<decimal> Withdraw(decimal amount)
    {
        if (amount <= 0)
            return new Error("InvalidAmount", "Withdrawal amount must be strictly greater than zero.");

        if (amount > Balance)
            return new Error("InsufficientFunds", $"Account {AccountNumber} has balance ${Balance}, cannot withdraw ${amount}.");

        Balance -= amount;
        return Balance; // Automatically converted to Result<decimal>.Success(Balance) via implicit operator!
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class ResultExample
{
    public static void Run()
    {
        Console.WriteLine("\n--- .NET Enterprise Pattern 4: The Result Pattern (ROP) ---");

        var account = new BankAccount("ACC-90210", 100m);

        // Attempt 1: Successful Withdrawal
        Console.WriteLine("1. Withdrawing $40:");
        var result1 = account.Withdraw(40m);
        PrintResult(result1);

        // Attempt 2: Domain Failure (Insufficient Funds)
        Console.WriteLine("\n2. Withdrawing $80 (Exceeds remaining $60 balance):");
        var result2 = account.Withdraw(80m);
        PrintResult(result2);

        // Attempt 3: Validation Failure (Negative Amount)
        Console.WriteLine("\n3. Withdrawing -$10:");
        var result3 = account.Withdraw(-10m);
        PrintResult(result3);
    }

    private static void PrintResult(Result<decimal> result)
    {
        result.Match(
            onSuccess: balance =>
            {
                Console.WriteLine($"  ✅ [SUCCESS] Remaining Account Balance: ${balance}");
                return true;
            },
            onFailure: error =>
            {
                Console.WriteLine($"  ❌ [FAILURE] Code: '{error.Code}' | Message: '{error.Description}'");
                return false;
            }
        );
    }
}
