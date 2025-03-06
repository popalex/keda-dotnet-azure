using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

[Route("api/videos")]
[ApiController]
public class VideoController : ControllerBase
{
    private readonly AzureStorageService _storageService;

    public VideoController(AzureStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadVideo([FromForm] VideoUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("No file uploaded");

        using var fileStream = request.File.OpenReadStream();
        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
        string videoUrl = await _storageService.UploadVideoAsync(fileStream, fileName);

        return Ok(new { Message = "Uploaded", VideoUrl = videoUrl });
    }

    [HttpGet]
    public async Task<IActionResult> GetVideos()
    {
        var videos = await _storageService.GetVideosAsync();
        return Ok(videos);
    }
}

public class VideoUploadRequest
{
    public IFormFile File { get; set; }
}
