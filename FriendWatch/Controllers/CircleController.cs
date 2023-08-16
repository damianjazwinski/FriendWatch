using System.Security.Claims;

using FriendWatch.Application.DTOs;
using FriendWatch.Application.Services;
using FriendWatch.DTOs.Requests;

using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok($"{id}");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCircleRequest request)
        {
            var currentUserId = int.Parse(HttpContext.User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

            var circleDto = new CircleDto 
            { 
                Name = request.Name 
            };

            await _circleService.CreateCircleAsync(circleDto, currentUserId);
            return Ok();
        }
    }
}
