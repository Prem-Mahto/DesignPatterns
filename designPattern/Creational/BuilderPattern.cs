namespace designPattern.Creational;

public class EmailMessage
{
    public string From { get; }
    public string To { get; }
    public string Subject { get; }
    public string? Body { get; }
    public bool IsHighPriority { get; }

    // Internal constructor: cannot be instantiated directly from outside
    internal EmailMessage(string from, string to, string subject, string? body, bool isHighPriority)
    {
        From = from;
        To = to;
        Subject = subject;
        Body = body;
        IsHighPriority = isHighPriority;
    }

    public void Print()
    {
        Console.WriteLine($"From: {From}");
        Console.WriteLine($"To: {To}");
        Console.WriteLine($"Subject: {Subject}");
        Console.WriteLine($"Body: {Body ?? "(empty)"}");
        Console.WriteLine($"High Priority: {IsHighPriority}");
    }
}

// -------------------------------------------------------------
// Step Builder Interfaces (Compile-time Type-State Machine)
// -------------------------------------------------------------

// Step 1: Only allows providing the recipient
public interface IRequireTo
{
    IRequireSubject To(string recipient);
}

// Step 2: Only allows providing the subject
public interface IRequireSubject
{
    IOptionalEmailSettings WithSubject(string subject);
}

// Step 3: All mandatory fields are satisfied! 
// Optional configurations or final .Build()
public interface IOptionalEmailSettings
{
    IOptionalEmailSettings WithBody(string body);
    IOptionalEmailSettings WithHighPriority();
    EmailMessage Build();
}

// -------------------------------------------------------------
// The Single Builder Class Implementing All Step Interfaces
// -------------------------------------------------------------
public class EmailBuilder : IRequireTo, IRequireSubject, IOptionalEmailSettings
{
    private readonly string _from;
    private string _to = string.Empty;
    private string _subject = string.Empty;
    private string? _body;
    private bool _isHighPriority;

    private EmailBuilder(string from)
    {
        _from = from;
    }

    // Static Entry Point: Demands "From" address first
    public static IRequireTo From(string from)
    {
        if (string.IsNullOrWhiteSpace(from))
            throw new ArgumentException("Sender is required.", nameof(from));

        return new EmailBuilder(from);
    }

    // Fulfills IRequireTo -> leads to IRequireSubject
    public IRequireSubject To(string recipient)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient is required.", nameof(recipient));

        _to = recipient;
        return this;
    }

    // Fulfills IRequireSubject -> leads to IOptionalEmailSettings
    public IOptionalEmailSettings WithSubject(string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject is required.", nameof(subject));

        _subject = subject;
        return this;
    }

    // Optional setting
    public IOptionalEmailSettings WithBody(string body)
    {
        _body = body;
        return this;
    }

    // Optional setting
    public IOptionalEmailSettings WithHighPriority()
    {
        _isHighPriority = true;
        return this;
    }

    // The final Build method
    public EmailMessage Build()
    {
        return new EmailMessage(_from, _to, _subject, _body, _isHighPriority);
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class BuilderExample
{
    public static void Run()
    {
        Console.WriteLine("=== BUILDER PATTERN (STEP BUILDER) ===");

        // IntelliSense strictly enforces From -> To -> WithSubject -> Build
        EmailMessage email = EmailBuilder
            .From("admin@company.com")
            .To("client@domain.com")
            .WithSubject("Invoice #1042 Ready")
            .WithBody("<h1>Please find your invoice attached.</h1>")
            .WithHighPriority()
            .Build();

        email.Print();
        Console.WriteLine();
    }
}
