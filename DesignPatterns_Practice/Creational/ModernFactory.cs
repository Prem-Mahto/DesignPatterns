using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns_Practice.Creational
{

    public class ModernOrderService
    {
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;
        private IServiceProvider _serviceProvider { get; }

        public ModernOrderService(IServiceProvider serviceProvider)
        {
            _paymentProcessorFactory = new PaymentProcessorFactory();
            _serviceProvider = serviceProvider;
        }


        public void ProcessOrder(Order order, PaymentMethod paymentMethod)
        {
            //IPaymentProcessor processor = _serviceProvider.GetRequiredKeyedService<IPaymentProcessor>(paymentMethod);
            IPaymentProcessor processor = _serviceProvider.GetKeyedService<IPaymentProcessor>(paymentMethod);
            if (processor == null)
            {
                Console.WriteLine($"Payment method {paymentMethod} is not supported.");
                return;
            }

            processor.Charge(order.TotalAmount);
        }

       
    }
    public class ModernFactoryExample
    {
        private readonly ModernOrderService OrderService;

        public ModernFactoryExample()
        {
            // create a host builder
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();

            builder.Services.AddSingleton<ModernOrderService>();
            builder.Services.AddKeyedScoped<IPaymentProcessor, CreditCardProcessor>(PaymentMethod.CreditCard);
            builder.Services.AddKeyedScoped<IPaymentProcessor, PayPalProcessor>(PaymentMethod.PayPal);
            builder.Services.AddKeyedScoped<IPaymentProcessor, ApplePayProcessor>(PaymentMethod.ApplePay);
            builder.Services.AddKeyedScoped<IPaymentProcessor, CryptoProcessor>(PaymentMethod.Crypto);


            Console.WriteLine("Modern Factory Example:");
            var serviceProvider = builder.Services.BuildServiceProvider();
            OrderService = serviceProvider.GetRequiredService<ModernOrderService>();
        }

        public static void Run()
        {
            var Modernfactory= new ModernFactoryExample();
            var orderService = Modernfactory.OrderService;
            orderService.ProcessOrder(new Order { TotalAmount = 100.00m }, PaymentMethod.CreditCard);
            orderService.ProcessOrder(new Order { TotalAmount = 50.00m }, PaymentMethod.PayPal);
            orderService.ProcessOrder(new Order { TotalAmount = 75.00m }, PaymentMethod.ApplePay);
            orderService.ProcessOrder(new Order { TotalAmount = 500.00m }, PaymentMethod.Crypto);
            orderService.ProcessOrder(new Order { TotalAmount = 700.00m }, PaymentMethod.NotSupported);
        }
    }
}
