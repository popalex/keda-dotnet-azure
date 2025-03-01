using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;

class Program
{
    static async Task Main()
    {
        string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        string queueName = "my-events";

        var queueClient = new QueueClient(connectionString, queueName);
        await queueClient.CreateIfNotExistsAsync();

        for (int i = 1; i <= 10; i++)
        {
            string message = $"Event {i} at {DateTime.UtcNow}";
            await queueClient.SendMessageAsync(message);
            Console.WriteLine($"Sent: {message}");
            await Task.Delay(1000);  // Simulate event generation delay
        }
    }
}
