using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DesignPatterns_Practice.Creational
{
    #region PaymentProcessors

    public enum PaymentMethod
    {
        CreditCard,
        PayPal,
        ApplePay,
        Crypto,
        NotSupported
    }

    // The common contract
    public interface IPaymentProcessor
    {
        PaymentMethod Method { get; }
        void Charge(decimal amount);
    }

    // Concrete product 1
    public class CreditCardProcessor : IPaymentProcessor
    {
        public PaymentMethod Method { get { return PaymentMethod.CreditCard; } }
        public void Charge(decimal amount) =>
            Console.WriteLine($"Charging ${amount} via Credit Card Gateway.");
    }

    // Concrete product 2
    public class PayPalProcessor : IPaymentProcessor
    {
        public PaymentMethod Method { get { return PaymentMethod.PayPal; } }

        public void Charge(decimal amount) =>
            Console.WriteLine($"Redirecting to PayPal to authorize ${amount}.");
    }

    // Concrete product 3
    public class ApplePayProcessor : IPaymentProcessor
    {
        public PaymentMethod Method { get { return PaymentMethod.ApplePay; } }

        public void Charge(decimal amount) =>
            Console.WriteLine($"Charging ${amount} via Apple Pay biometric token.");
    }

    // Concrete product 3
    public class CryptoProcessor : IPaymentProcessor
    {
        public PaymentMethod Method { get { return PaymentMethod.Crypto; } }

        public void Charge(decimal amount) =>
            Console.WriteLine($"Charging ${amount} via Crypto Wallet.");
    }


    #endregion

    #region factory
    public interface IPaymentProcessorFactory
    {
        IPaymentProcessor GetPaymentProcessor(PaymentMethod paymentMethod);
    }

    /// <summary>
    /// for true GoF Factory -> we will have separate concerete factory class for each Payment processor class.
    /// which instance of payment processor is created depends upon, factory class passed
    /// Problem:- explosion of sub class -> 20 subclass for 10 payment processor
    /// fix :- Modern factory
    /// </summary>
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {
        public IPaymentProcessor GetPaymentProcessor(PaymentMethod paymentMethod)
        {
            return paymentMethod switch
            {
                PaymentMethod.CreditCard => new CreditCardProcessor(),
                PaymentMethod.PayPal => new PayPalProcessor(),
                PaymentMethod.ApplePay => new ApplePayProcessor(),
                PaymentMethod.Crypto => new CryptoProcessor(),
                _ => throw new NotSupportedException("Invalid payment method")
            };
        }
    }

    public class ModernPaymentFactory : IPaymentProcessorFactory
    {
        private readonly Dictionary<string, IPaymentProcessor> _processors;

        public ModernPaymentFactory(IEnumerable<IPaymentProcessor> processors)
        {
            _processors = processors.ToDictionary(p => p.Method.ToString(), p => p, StringComparer.OrdinalIgnoreCase);
        }

        public IPaymentProcessor GetPaymentProcessor(PaymentMethod paymentMethod) =>
            _processors.TryGetValue(paymentMethod.ToString(), out var p) ? p : throw new NotSupportedException();
    }

    #endregion


    public class OrderService
    {
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;

        public OrderService(IPaymentProcessorFactory paymentProcessorFactory)
        {
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        public void ProcessOrder(Order order, PaymentMethod paymentMethod)
        {
            IPaymentProcessor processor = _paymentProcessorFactory.GetPaymentProcessor(paymentMethod);

            processor.Charge(order.TotalAmount);
        }
    }

    public class Order
    {
        public decimal TotalAmount { get; set; }
        // add other properties/members your code needs
    }

    public class FactoryExample
    {
        public static void Run()
        {
            var orderService = new OrderService(new PaymentProcessorFactory());

            orderService.ProcessOrder(new Order { TotalAmount = 100.00m }, PaymentMethod.CreditCard);
            orderService.ProcessOrder(new Order { TotalAmount = 50.00m }, PaymentMethod.PayPal);
            orderService.ProcessOrder(new Order { TotalAmount = 75.00m }, PaymentMethod.ApplePay);
            orderService.ProcessOrder(new Order { TotalAmount = 500.00m }, PaymentMethod.Crypto);


            var processors = new IPaymentProcessor[] {
                            new CreditCardProcessor(),
                            new PayPalProcessor(),
                            new ApplePayProcessor(),
                            new CryptoProcessor()
                        };

            var orderService2 = new OrderService(new ModernPaymentFactory(processors));

            orderService2.ProcessOrder(new Order { TotalAmount = 100.00m }, PaymentMethod.CreditCard);
            orderService2.ProcessOrder(new Order { TotalAmount = 50.00m }, PaymentMethod.PayPal);
            orderService2.ProcessOrder(new Order { TotalAmount = 75.00m }, PaymentMethod.ApplePay);
            orderService2.ProcessOrder(new Order { TotalAmount = 500.00m }, PaymentMethod.Crypto);
        }

    }
}
