using FriendWatch.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            //var userResponse = new
            //{
            //    Id = user.Id,
            //    Username = user.Username,
            //    OwnedCircles = user.OwnedCircles,
            //    JoinedCircles = user.Circles,
            //    ReceivedInvitations = user.ReceivedInvitations
            //};

            return Ok(user);
        }
    }
}
