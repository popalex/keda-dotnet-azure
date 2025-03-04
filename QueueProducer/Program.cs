using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;

class Program
{
    static async Task Main()
    {
        string storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        string queueName = "image-processing-queue";
        string containerName = "images";

        var queueClient = new QueueClient(storageConnectionString, queueName);
        var blobClient = new BlobContainerClient(storageConnectionString, containerName);

        await queueClient.CreateIfNotExistsAsync();
        await blobClient.CreateIfNotExistsAsync();

        string filePath = "sample.jpg";  // Local image file to upload
        string blobName = Guid.NewGuid().ToString() + Path.GetExtension(filePath);
        BlobClient blob = blobClient.GetBlobClient(blobName);

        await blob.UploadAsync(filePath);
        Console.WriteLine($"Uploaded: {blobName}");

        await queueClient.SendMessageAsync(blobName);
        Console.WriteLine($"Sent event: {blobName}");
    }
}
