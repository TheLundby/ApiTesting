using API;
using Microsoft.AspNetCore.Mvc;
using Application.Handlers;
using Domain;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private LoginHandler _loginHandler;

        public AuthController(LoginHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            string? token;
            try
            {
                token = _loginHandler.Handle(credentials);

                if (token == null)
                {
                    return BadRequest("Incorrect username or password");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new { token });
        }
    }
}
