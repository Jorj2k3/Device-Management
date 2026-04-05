using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services;

namespace DeviceManagement.Api.Controllers
{
    /// <summary>
    /// UserController is responsible for handling HTTP requests related to user management,
    /// including creating, retrieving, updating, and deleting device records. 
    /// It interacts with the IUserService to perform these operations and returns appropriate HTTP responses based on the outcome of each request.
    /// </summary>

    [Route("api/[controller]s")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var usersDTO = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                Location = u.Location
            });

            return Ok(usersDTO);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var usersDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Location = user.Location
            };

            return Ok(usersDTO);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userDTO)
        {
            var newUser = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Role = "Employee",
                Location = userDTO.Location
            };

            var createdUser = await _userService.CreateUserAsync(newUser);

            userDTO.Id = createdUser.Id;
            userDTO.Role = createdUser.Role;

            return CreatedAtAction(nameof(GetUser), new { id = userDTO.Id }, userDTO);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO userDTO)
        {
            if (id != userDTO.Id) return BadRequest("The ID does not match the request body.");

            var userToUpdate = new User
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                Email = userDTO.Email,
                Location = userDTO.Location
            };

            var updatedUser = await _userService.UpdateUserAsync(id, userToUpdate);

            if (updatedUser == null) return NotFound();

            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
