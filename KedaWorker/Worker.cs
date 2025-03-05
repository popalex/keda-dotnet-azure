using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoSauce.MagicScaler;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly QueueClient _queueClient;
    private readonly BlobContainerClient _blobContainerClient;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        string storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        string queueName = "image-processing-queue";
        string containerName = "images";

        _queueClient = new QueueClient(storageConnectionString, queueName);
        _blobContainerClient = new BlobContainerClient(storageConnectionString, containerName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await _queueClient.ReceiveMessageAsync();
            if (message.Value != null)
            {
                string blobName = message.Value.MessageText;
                _logger.LogInformation($"Processing image: {blobName}");

                await ProcessImage(blobName);
                await _queueClient.DeleteMessageAsync(message.Value.MessageId, message.Value.PopReceipt);
            }
            else
            {
                await Task.Delay(2000);
            }
        }
    }

    private async Task ProcessImage(string blobName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        var outputClient = _blobContainerClient.GetBlobClient($"processed-{blobName}");

        using (var inputStream = new MemoryStream())
        {
            await blobClient.DownloadToAsync(inputStream);
            inputStream.Position = 0;

            using (var outputStream = new MemoryStream())
            {
                MagicImageProcessor.ProcessImage(inputStream, outputStream, new ProcessImageSettings
                {
                    Width = 100,
                    Height = 100,
                    ResizeMode = CropScaleMode.Max
                });

                outputStream.Position = 0;
                await outputClient.UploadAsync(outputStream);
            }
        }

        _logger.LogInformation($"Processed & saved: processed-{blobName}");
    }
}
