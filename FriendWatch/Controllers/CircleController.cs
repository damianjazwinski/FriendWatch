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
using Azure.Core;
using static System.Net.Mime.MediaTypeNames;

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
        [HttpGet("owned/{id}")]
        public async Task<IActionResult> GetOwnedCircle(int id)
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();
            var result = await _circleService.GetByIdWithMembersAsync(id, currentUserId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));

            return Ok(new GetCircleWithMembers { Circle = result.Data! });
        }

        [Authorize]
        [HttpGet("joined/{id}")]
        public async Task<IActionResult> GetJoinedCircle(int id)
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();
            var result = await _circleService.GetByIdWithMembersAsync(id, currentUserId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));

            return Ok(new GetCircleWithMembers { Circle = result.Data! });
        }

        [Authorize]
        [HttpGet("owned")]
        public async Task<IActionResult> GetOwned()
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();
            var result = await _circleService.GetOwnedCirclesAsync(currentUserId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));

            return Ok(new GetOwnedCirclesResponse { OwnedCircles = result.Data! });
        }

        [Authorize]
        [HttpGet("joined")]
        public async Task<IActionResult> GetJoined()
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();
            var getJoinedCirclesResponse = await _circleService.GetJoinedCirclesAsync(currentUserId);

            if (!getJoinedCirclesResponse.IsSuccess)
                return BadRequest(new ErrorResponse(getJoinedCirclesResponse.Message!));

            return Ok(new GetJoinedCirclesResponse { JoinedCircles = getJoinedCirclesResponse.Data! });
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCircleRequest request)
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();

            var circleDto = new CircleDto
            {
                Name = request.Name,
            };

            if (request.Image != null)
            {
                if (request.Image.Length == 0)
                    return BadRequest(new ErrorResponse("Image file is empty"));

                if (request.Image.Length > 5242880)
                    return BadRequest(new ErrorResponse("Image file is larger than 5MB"));

                using var memoryStream = new MemoryStream();
                await request.Image.CopyToAsync(memoryStream);

                circleDto.ImageFile = new ImageFileDto
                {
                    FileName = request.Image.FileName,
                    ContentType = request.Image.ContentType,
                    Data = memoryStream.ToArray(),
                };
            }

            var result = await _circleService.CreateCircleAsync(circleDto, currentUserId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));

            return Ok(new SuccessResponse());
        }
    }
}
