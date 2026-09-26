using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace DesignPatterns_Practice.Creational
{
    public class BuilderExample
    {
        public static void Run()
        {
            var emailMessage = EmailMessageBuilder.From("sender@example.com")
                
                
                .To("recipient@example.com")
                .Subject("Hello").Body("This is a test email.").Html().Build();
            Console.WriteLine($"To: {string.Join(", ", emailMessage.To)}");
            Console.WriteLine($"From: {emailMessage.From}");
            Console.WriteLine($"Subject: {emailMessage.Subject}");
            Console.WriteLine($"Body: {emailMessage.Body}");
            Console.WriteLine($"Is HTML: {emailMessage.IsHtml}");
        }

    }

    interface IEmailSender
    {

    }
    public class EmailMessage : IEmailSender
    {
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        public EmailMessage(List<string> to, List<string> cc, List<string> bcc, string from, string subject, string body, bool isHtml)
        {
            To = to;
            Cc = cc;
            Bcc = bcc;
            From = from;
            Subject = subject;
            Body = body;
            IsHtml = isHtml;
        }
    }

    public interface IEmailBuilderTo
    {
       IEmailBuilderSubject To(string email);
    }

    //public interface IEmailBuilderFrom
    //{
    //    IEmailBuilderSubject From(string email);
    //}
    public interface IEmailBuilderSubject
    {
        IEmailBuilder Subject(string subject);
    }
    public interface IEmailBuilder
    {
        IEmailBuilder Cc(string email);

        IEmailBuilder Bcc(string email);

        IEmailBuilder Body(string body);

        IEmailBuilder Html();

        EmailMessage Build();
    }

    //1 privaate constructor+ 1 static method -> single entry, then interfaces to force only next methods
    public class EmailMessageBuilder : IEmailBuilderTo, IEmailBuilderSubject, IEmailBuilder
    {
        private List<string> _to = new List<string>();
        private List<string> _cc = new List<string>();
        private List<string> _bcc = new List<string>();
        private string _from;
        private string _subject;
        private string _body;
        private bool _isHtml;

        private EmailMessageBuilder(string from)
        {
            _from = from;
        }

        public static IEmailBuilderTo From(string email)
        {
            return new EmailMessageBuilder(email);
        }
        public IEmailBuilderSubject To(string email)
        {
            _to.Add(email);
            return this;
        }
        public IEmailBuilder Subject(string subject)
        {
            _subject = subject;
            return this;
        }


        public IEmailBuilder Cc(string email)
        {
            _cc.Add(email);
            return this;
        }

        public IEmailBuilder Bcc(string email)
        {
            _bcc.Add(email);
            return this;
        }

        public IEmailBuilder Body(string body)
        {
            _body = body;
            return this;
        }

        public IEmailBuilder Html()
        {
            _isHtml = true;
            return this;
        }

        public EmailMessage Build()
        {
            if (string.IsNullOrEmpty(_from) || !_to.Any() || string.IsNullOrEmpty(_subject))
            {
                throw new InvalidOperationException("From address, to address, and subject are required.");
            }
            return new EmailMessage(_to, _cc, _bcc, _from, _subject, _body, _isHtml);
        }

    }
}
