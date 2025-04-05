using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyWebAPI.Services;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, ITokenService tokenService, IUserService userService)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginUserWithEmailAndPassword(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var tokens = await _tokenService.GenerateAuthTokens(user);
            tokens.RefreshToken = ""; // Hiding refresh token

            return Ok(new
            {
                User = new { user.Id, user.Email, user.FirstName, user.LastName },
                Tokens = tokens
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _userService.CreateUser(request);
            if (user == null)
            {
                return Conflict(new { message = "User already exists" });
            }

            var tokens = await _tokenService.GenerateAuthTokens(user);
            tokens.RefreshToken = ""; // Hiding refresh token

            return Ok(new { user, tokens });
        }
    }
}
