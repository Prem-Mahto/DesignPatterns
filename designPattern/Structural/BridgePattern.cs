namespace designPattern.Structural;

// 1. The Implementation Dimension (Platform / Delivery Channel)
public interface IMessageSender
{
    void SendMessage(string title, string body);
}

public class EmailSender : IMessageSender
{
    public void SendMessage(string title, string body) =>
        Console.WriteLine($"[EMAIL] Subject: '{title}' | Body: {body}");
}

public class SmsSender : IMessageSender
{
    public void SendMessage(string title, string body) =>
        Console.WriteLine($"[SMS] Alert: {title} - {body}");
}

public class SlackSender : IMessageSender
{
    public void SendMessage(string title, string body) =>
        Console.WriteLine($"[SLACK #alerts] {title}: {body}");
}

// 2. The Abstraction Dimension (Business Notification Concept)
public abstract class Notification
{
    // The Bridge to the implementation!
    protected readonly IMessageSender _sender;

    protected Notification(IMessageSender sender)
    {
        _sender = sender;
    }

    public abstract void Notify(string message);
}

public class SystemAlertNotification : Notification
{
    public SystemAlertNotification(IMessageSender sender) : base(sender) { }

    public override void Notify(string message)
    {
        string title = "CRITICAL ALERT";
        string formattedBody = $"[Timestamp: {DateTime.UtcNow}] {message.ToUpperInvariant()}";
        _sender.SendMessage(title, formattedBody);
    }
}

public class UserReminderNotification : Notification
{
    public UserReminderNotification(IMessageSender sender) : base(sender) { }

    public override void Notify(string message)
    {
        string title = "Friendly Reminder";
        string formattedBody = $"Hi there! Just reminding you: {message}";
        _sender.SendMessage(title, formattedBody);
    }
}

// -------------------------------------------------------------
// Runner Example
// -------------------------------------------------------------
public class BridgeExample
{
    public static void Run()
    {
        Console.WriteLine("=== BRIDGE PATTERN ===");

        // 1. Critical Alert via SMS
        Notification urgentAlert = new SystemAlertNotification(new SmsSender());
        urgentAlert.Notify("Database replica is offline!");

        // 2. Same Critical Alert via Slack
        Notification slackAlert = new SystemAlertNotification(new SlackSender());
        slackAlert.Notify("CPU usage exceeded 95%!");

        // 3. Reminder via Email
        Notification emailReminder = new UserReminderNotification(new EmailSender());
        emailReminder.Notify("Your subscription renews tomorrow.");

        Console.WriteLine();
    }
}
