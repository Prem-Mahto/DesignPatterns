using System;

namespace DesignPatterns_Practice.Creational;

/// <summary>
/// Factory for related objects family
/// </summary>

///The Abstract Product Interfaces (The Family Members)
#region The Family Members
/// Family Member 1
public interface IFileStorage
{
    void UploadFile(string fileName, byte[] fileData);
}
// Family Member 2
public interface IMessageQueue
{
    void EnqueueMessage(string message);
}
//Family Member 3
public interface IAuditDatabase
{
    void RecordLog(string action);
}
#endregion

#region The AWS Family
public class AwsS3Storage : IFileStorage
{
    public void UploadFile(string fileName, byte[] data) =>
        Console.WriteLine($"📦 [AWS S3] Uploaded '{fileName}' ({data.Length} bytes) to bucket 'production-docs'.");
}

public class AwsSqsQueue : IMessageQueue
{
    public void EnqueueMessage(string message) =>
        Console.WriteLine($"📬 [AWS SQS] Enqueued message to 'arn:aws:sqs:us-east-1:order-events': \"{message}\"");
}

public class AwsDynamoDatabase : IAuditDatabase
{
    public void RecordLog(string action) =>
        Console.WriteLine($"🗄️ [AWS DynamoDB] Wrote audit entry: '{action}' to table 'SystemAuditLog'.");
}
#endregion

#region The Azure Family
public class AzureBlobStorage : IFileStorage
{
    public void UploadFile(string fileName, byte[] data) =>
        Console.WriteLine($"📦 [Azure Blob Storage] Uploaded '{fileName}' ({data.Length} bytes) to container 'production-docs'.");
}

public class AzureServiceBusQueue : IMessageQueue
{
    public void EnqueueMessage(string message) =>
        Console.WriteLine($"📬 [Azure Service Bus] Enqueued message to 'order-events': \"{message}\"");
}
public class AzureCosmosDatabase : IAuditDatabase
{
    public void RecordLog(string action) =>
        Console.WriteLine($"🗄️ [Azure Cosmos DB] Wrote audit entry: '{action}' to collection 'SystemAuditLog'.");
}

#endregion

/// <summary>
/// The Abstract Factory Interface
/// </summary>
public interface ICloudServiceFactory
{
    IFileStorage CreateStorage();
    IMessageQueue CreateQueue();
    IAuditDatabase CreateDatabase();
}

public class AwsServiceFactory : ICloudServiceFactory
{
    public IFileStorage CreateStorage()=> new AwsS3Storage();
    public IAuditDatabase CreateDatabase() => new AwsDynamoDatabase();
    public IMessageQueue CreateQueue()=> new AwsSqsQueue();
}

public class AzureServiceFactory : ICloudServiceFactory
{
    public IFileStorage CreateStorage()=> new AzureBlobStorage();
    public IAuditDatabase CreateDatabase() => new AzureCosmosDatabase();
    public IMessageQueue CreateQueue()=> new AzureServiceBusQueue();
}

public class DocumentWorkFlowManager
{
    private readonly IFileStorage _storage;
    private readonly IMessageQueue _queue; 
    private readonly IAuditDatabase _database;

    public DocumentWorkFlowManager(ICloudServiceFactory factory)
    {
        _storage = factory.CreateStorage();
        _queue = factory.CreateQueue();
        _database = factory.CreateDatabase();
    }

    public void ProcessDocument(string fileName, byte[] data)
    {
        Console.WriteLine($"\n--- Processing Document: {fileName} ---");
        _storage.UploadFile(fileName, data);
        _queue.EnqueueMessage($"FileReady:{fileName}");
        _database.RecordLog($"Uploaded and enqueued {fileName} at {DateTime.UtcNow:HH:mm:ss}");
    }
}

public class AbstractFactoryExample
{
    public static void Run()
    {
        byte[] sampleData = new byte[]{0x25, 0x50, 0x44, 0x46};

        Console.WriteLine("=== Deployement 1: AWS Environment ===");
        var awsFactory = new AwsServiceFactory();
        var awsManager = new DocumentWorkFlowManager(awsFactory);
        awsManager.ProcessDocument("Quarterly_Report.pdf", sampleData); 

        Console.WriteLine("\n=== Deployement 2: Azure Environment ===");
        var azureFactory = new AzureServiceFactory();
        var azureManager = new DocumentWorkFlowManager(azureFactory);
        azureManager.ProcessDocument("Annual_Summary.pdf", sampleData);
    }
}