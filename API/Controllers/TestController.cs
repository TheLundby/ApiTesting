using API;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("ok")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}
