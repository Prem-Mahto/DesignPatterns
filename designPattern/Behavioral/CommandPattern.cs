namespace designPattern.Behavioral;

// 1. Command Interface
public interface ITransactionCommand
{
    bool Execute();
    void Undo();
}

// 2. Receiver (Bank Account)
public class BankAccount
{
    public string AccountNumber { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"[Account {AccountNumber}] Deposited ${amount}. New Balance: ${Balance}");
    }

    public bool Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"[Account {AccountNumber}] Withdrew ${amount}. New Balance: ${Balance}");
            return true;
        }

        Console.WriteLine($"[Account {AccountNumber}] ❌ Insufficient funds to withdraw ${amount}!");
        return false;
    }
}

// 3. Concrete Commands
public class DepositCommand : ITransactionCommand
{
    private readonly BankAccount _account;
    private readonly decimal _amount;
    private bool _isExecuted = false;

    public DepositCommand(BankAccount account, decimal amount)
    {
        _account = account;
        _amount = amount;
    }

    public bool Execute()
    {
        _account.Deposit(_amount);
        _isExecuted = true;
        return true;
    }

    public void Undo()
    {
        if (_isExecuted)
        {
            Console.WriteLine($"--> [UNDO] Reversing Deposit of ${_amount}...");
            _account.Withdraw(_amount);
            _isExecuted = false;
        }
    }
}

public class WithdrawCommand : ITransactionCommand
{
    private readonly BankAccount _account;
    private readonly decimal _amount;
    private bool _isExecuted = false;

    public WithdrawCommand(BankAccount account, decimal amount)
    {
        _account = account;
        _amount = amount;
    }

    public bool Execute()
    {
        _isExecuted = _account.Withdraw(_amount);
        return _isExecuted;
    }

    public void Undo()
    {
        if (_isExecuted)
        {
            Console.WriteLine($"--> [UNDO] Reversing Withdrawal of ${_amount}...");
            _account.Deposit(_amount);
            _isExecuted = false;
        }
    }
}

// 4. Invoker / Transaction Manager (Manages Undo Stack)
public class TransactionManager
{
    private readonly Stack<ITransactionCommand> _history = new();

    public void ExecuteTransaction(ITransactionCommand command)
    {
        if (command.Execute())
        {
            _history.Push(command);
        }
    }

    public void UndoLastTransaction()
    {
        if (_history.Count > 0)
        {
            var command = _history.Pop();
            command.Undo();
        }
        else
        {
            Console.WriteLine("No transactions left to undo!");
        }
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class CommandExample
{
    public static void Run()
    {
        Console.WriteLine("=== COMMAND PATTERN (WITH UNDO) ===");

        var account = new BankAccount("US-9912", initialBalance: 100m);
        var manager = new TransactionManager();

        Console.WriteLine("--- Executing Transactions ---");
        manager.ExecuteTransaction(new DepositCommand(account, 50m));
        manager.ExecuteTransaction(new WithdrawCommand(account, 30m));
        manager.ExecuteTransaction(new WithdrawCommand(account, 200m)); // Should fail gracefully

        Console.WriteLine("\n--- Performing Undo (Rollbacks) ---");
        manager.UndoLastTransaction(); // Undoes $30 withdrawal
        manager.UndoLastTransaction(); // Undoes $50 deposit
        manager.UndoLastTransaction(); // Empty history check

        Console.WriteLine();
    }
}
