﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<VideosController> logger;

    public VideosController(IWebHostEnvironment env, ILogger<VideosController> logger)
    {
        this.env = env;
        this.logger = logger;
    }

    [HttpGet("{video}")]
    public IActionResult GetVideo(string video)
    {
        var mime = video.Split('.').Last();
        var savePath = Path.Combine(env.WebRootPath, "videos", video);
        return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.ReadWrite), MediaTypeHeaderValue.Parse("video/*"));
    }
    [HttpPost]
    public async Task<IActionResult> UploadVideo(IFormFile video)
    {
        var mime = video.FileName.Split('.').Last();
        var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
        var savePath = Path.Combine(env.WebRootPath, "videos", fileName);
        await using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
        {
            await video.CopyToAsync(fileStream);
        }

        await Task.Run(() =>
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(env.ContentRootPath, "FFMPEG", "ffmpeg.exe"),
                Arguments = $"-y -i {savePath} -an -vf scale=540x380 {Path.Combine(env.WebRootPath,"videos", "test.mp4")}",
                CreateNoWindow = true,
                UseShellExecute = false,
            };
            using var process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();
            process.Dispose();
        });

        return Ok(fileName);
    }
}
