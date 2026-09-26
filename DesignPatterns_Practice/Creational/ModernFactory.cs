using Microsoft.Extensions.DependencyInjection;
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
        private readonly ModernOrderService orderService;

        public ModernFactoryExample(ModernOrderService modernOrderService)
        {
            orderService = modernOrderService;
        }

        public void Run()
        {
            orderService.ProcessOrder(new Order { TotalAmount = 100.00m }, PaymentMethod.CreditCard);
            orderService.ProcessOrder(new Order { TotalAmount = 50.00m }, PaymentMethod.PayPal);
            orderService.ProcessOrder(new Order { TotalAmount = 75.00m }, PaymentMethod.ApplePay);
            orderService.ProcessOrder(new Order { TotalAmount = 500.00m }, PaymentMethod.Crypto);
            orderService.ProcessOrder(new Order { TotalAmount = 700.00m }, PaymentMethod.NotSupported);
        }
    }
}
