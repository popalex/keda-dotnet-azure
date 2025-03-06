using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class AzureStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _videoContainer;
    private readonly string _thumbnailContainer;
    private readonly string _blobBaseUrl;
    private readonly ILogger<AzureStorageService> _logger;

    public AzureStorageService(ILogger<AzureStorageService> logger, IConfiguration config)
    {
        _logger = logger;
        
        var connectionString = config["AzureStorage:ConnectionString"];
        _blobServiceClient = new BlobServiceClient(connectionString);
        _videoContainer = config["AzureStorage:VideoContainer"];
        _thumbnailContainer = config["AzureStorage:ThumbnailContainer"];
        _blobBaseUrl = config["AzureStorage:BlobBaseUrl"];
    }

    public async Task<string> UploadVideoAsync(Stream fileStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_videoContainer);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(fileStream, true);
        return $"{_blobBaseUrl}/{_videoContainer}/{fileName}";
    }

    public async Task<List<VideoDto>> GetVideosAsync()
    {
        var videoContainer = _blobServiceClient.GetBlobContainerClient(_videoContainer);
        await videoContainer.CreateIfNotExistsAsync();
        var thumbnailContainer = _blobServiceClient.GetBlobContainerClient(_thumbnailContainer);
        await thumbnailContainer.CreateIfNotExistsAsync();
        var videos = new List<VideoDto>();

        await foreach (BlobItem blobItem in videoContainer.GetBlobsAsync())
        {
            string thumbnailName = $"thumb-{Path.GetFileNameWithoutExtension(blobItem.Name)}.jpg";
            string videoUrl = $"{_blobBaseUrl}/{_videoContainer}/{blobItem.Name}";
            string thumbnailUrl = $"{_blobBaseUrl}/{_thumbnailContainer}/{thumbnailName}";

            videos.Add(new VideoDto
            {
                FileName = blobItem.Name,
                VideoUrl = videoUrl,
                ThumbnailUrl = thumbnailUrl
            });
        }
        return videos;
    }
}

public class VideoDto
{
    public string FileName { get; set; }
    public string VideoUrl { get; set; }
    public string ThumbnailUrl { get; set; }
}
