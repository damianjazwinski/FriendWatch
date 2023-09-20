using FriendWatch.Application.Services;
using FriendWatch.DTOs.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IImageFileService _fileService;

        public DownloadController(IImageFileService fileService)
        {
            _fileService = fileService;
        }

        [Authorize]
        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetImage(string fileName)
        {
            var result = await _fileService.GetFileByNameAsync(fileName);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));

            var imageFile = result.Data;

            if (imageFile == null)
                return BadRequest(new ErrorResponse("Download file failed"));

            var imagePath = Path.Combine(Environment.CurrentDirectory, "files", imageFile.FileName!);

            Stream stream = System.IO.File.OpenRead(imagePath);

            if (stream == null)
                return BadRequest(new ErrorResponse("Download file failed"));

            return new FileStreamResult(stream, imageFile.ContentType!);
        }
    }
}
