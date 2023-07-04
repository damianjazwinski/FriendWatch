using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [Route("api/[controller]")]
    public class WatchController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Test");
        }
    }
}
