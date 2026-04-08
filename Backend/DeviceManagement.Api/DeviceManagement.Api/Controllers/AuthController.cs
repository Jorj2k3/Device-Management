using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Services;
using DeviceManagement.Api.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DeviceManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IUserService userService, ITokenGenerator tokenGenerator)
        {
            _userService = userService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(request.Email);

                if (user == null || user.PasswordHash != request.Password)
                {
                    return Unauthorized("Invalid email or password.");
                }

                var token = _tokenGenerator.GenerateToken(user);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred during login.");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var existingUser = await _userService.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest("An account with this email already exists.");
            }

            var newUser = new Models.User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.Password,
                Role = "Employee",
                Location = request.Location
            };

            await _userService.CreateUserAsync(newUser);

            return Ok(new { message = "Registration successful" });
        }
    }
}
