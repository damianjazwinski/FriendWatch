using FriendWatch.DTOs.Requests;
using FriendWatch.Services.CircleService;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CirclesController : ControllerBase
    {
        private readonly ICircleService _circleService;
        public CirclesController(ICircleService circleService)
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
            await _circleService.CreateCircleAsync(request);
            return Ok();
        }
    }
}
