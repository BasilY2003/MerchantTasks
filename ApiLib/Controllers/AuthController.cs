using CommonLib.Localization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly CommonLib.Services.AuthenticationService _authService;

        public AuthController(CommonLib.Services.AuthenticationService authService)
        {
            _authService = authService;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest req)
        {
            var success = await _authService.RegisterAsync(req.Email, req.Password);
            if (!success)
            {
                var message = LocalizedMessage.GetMessage("TakenUserName");
                var error = new ErrorResponse
                {
                    ErrorCode = ErrorCode.NotFound,
                    ErrorMessage = message,
                    Details = null,
                };
                return BadRequest(error);
            }
            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var token = await _authService.LoginAsync(req.Email, req.Password);
            if (token == null)
            {
                var message = LocalizedMessage.GetMessage("LoginCredentials");
                var error = new ErrorResponse
                {
                    ErrorCode = ErrorCode.NotFound,
                    ErrorMessage = message,
                    Details = null,
                };
                return Unauthorized(error);
            }
            return Ok(new { token });
        }
    }
}
