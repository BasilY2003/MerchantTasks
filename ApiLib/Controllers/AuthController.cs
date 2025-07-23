using CommonLib.Interfaces;
using CommonLib.Localization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest req)
        {
            var success = await _authService.RegisterAsync(req.Email, req.Password);
          
            if (!success) return AlreadyUsedUsername(LocalizedMessage.GetMessage("TakenUserName"));
            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var token = await _authService.LoginAsync(req.Email, req.Password);

            if (token == null) return UnauthorizedLoginResponse();
            return Ok(new { token });
        }
    }
}
