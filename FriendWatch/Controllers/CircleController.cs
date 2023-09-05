using System.IO.Compression;
using System.Security.Claims;

using FriendWatch.Application.DTOs;
using FriendWatch.Application.Services;
using FriendWatch.Application.Extensions;
using FriendWatch.DTOs.Requests;
using FriendWatch.DTOs.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CircleController : ControllerBase
    {
        private readonly ICircleService _circleService;
        public CircleController(ICircleService circleService)
        {
            _circleService = circleService;
        }

        [Authorize]
        [HttpGet("owned")]
        public async Task<IActionResult> Get()
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();
            var result = await _circleService.GetOwnedCirclesAsync(currentUserId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse { Messages = new string[] { result.Message } });

            return Ok(new GetOwnedCirclesResponse { OwnedCircles = result.Data });
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateCircleRequest request)
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();

            var circleDto = new CircleDto 
            { 
                Name = request.Name 
            };

            if (request.Image != null && request.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await request.Image.CopyToAsync(memoryStream);
                    // Upload the file if less than 5 MB
                    if (memoryStream.Length < 5242880)
                    {
                        circleDto.Image = memoryStream.ToArray();
                        circleDto.Extension = Path.GetExtension(request.Image.FileName);
                    }
                    else
                    {
                        return BadRequest(new ErrorResponse { Messages = new string[] { "Image file is larger than 5MB" } });
                    }
                }
            }

            var result = await _circleService.CreateCircleAsync(circleDto, currentUserId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse { Messages = new string[] { result.Message } });

            return Ok(new SuccessResponse());
        }
    }
}
