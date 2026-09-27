using System;

namespace DesignPatterns_Practice.Structural;
public interface IMessageSender
{
    void SendMessage(string message);
}

public class SmsSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending SMS: {message}");
    }
}
public class EmailSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending Email: {message}");
    }
}
public class SlackSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending Slack message: {message}");
    }
}

public abstract class Notification
{
    protected readonly IMessageSender _messageSender;

    public Notification(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public abstract void Notify(string message);
}

public class UserReminderNotification : Notification
{
    public UserReminderNotification(IMessageSender messageSender) : base(messageSender) { }

    public override void Notify(string message)
    {
        _messageSender.SendMessage($"User Reminder: {message}");
    }
}

public class SystemAlertNotification : Notification
{
    public SystemAlertNotification(IMessageSender messageSender) : base(messageSender) { }

    public override void Notify(string message)
    {
        _messageSender.SendMessage($"System Alert: {message}");
    }
}

public class BridgeExample
{
    public static void Run()
    {
        var SmsUserReminder = new UserReminderNotification(new SmsSender());
        SmsUserReminder.Notify("Your subscription is about to expire!");

        var slackSystemAlert = new SystemAlertNotification(new SlackSender());
        slackSystemAlert.Notify("Server CPU usage is critically high!");
    }    
}