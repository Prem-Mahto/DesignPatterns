using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns_Practice.Structural
{
    interface INotificationService
    {
        Task SendAsync(string recipient, string message);
    }

    class SpeedySmsLegacyApi
    {
        public bool DispatchMessageV2(long internationalPhoneNumber, string text, int priorityFlag, bool isFlash)
        {
            Console.WriteLine($"[Vendor API] Dispatched to +{internationalPhoneNumber}: \"{text}\" (Priority: {priorityFlag})");
            return true;
        }
    }

    class SpeedySmsAdapter : INotificationService
    {
        private readonly SpeedySmsLegacyApi LegacyApi;

        public SpeedySmsAdapter(SpeedySmsLegacyApi legacyApi)
        {
            this.LegacyApi = legacyApi;
        }
        public Task SendAsync(string recipient, string message)
        {
            // 1. Adapt and translate inputs: strip out non-digits
            string cleanPhone = new string(recipient.Where(char.IsDigit).ToArray());

            if (!long.TryParse(cleanPhone, out long parsedPhone))
            {
                throw new ArgumentException($"Invalid phone number format: {recipient}");
            }

            // 2. Map standard call to vendor parameters
            int normalPriority = 1;
            bool isFlash = false;

            bool success = LegacyApi.DispatchMessageV2(parsedPhone, message, normalPriority, isFlash);

            if (!success)
            {
                throw new InvalidOperationException("Failed to dispatch SMS through vendor gateway.");
            }

            return Task.CompletedTask;
        }
    }


    class OrderService
    {
        private readonly INotificationService notifier;

        public OrderService(INotificationService notifier)
        {
            this.notifier = notifier;
        }
        public async Task CompleteOrderAsync(string customerPhone, decimal amount)
        {
            Console.WriteLine($"Order of ${amount} completed.");
            await notifier.SendAsync(customerPhone, $"Your order of ${amount} is confirmed!");
        }
    }
    
    public class AdaptorExample
    {
        public static void Run()
        {
            var adapter = new SpeedySmsAdapter(new SpeedySmsLegacyApi());

            var orderService = new OrderService(adapter);
            orderService.CompleteOrderAsync("+1 (555) 019-2834", 199.99m).GetAwaiter().GetResult();

            Console.WriteLine();
        }
    }
}
