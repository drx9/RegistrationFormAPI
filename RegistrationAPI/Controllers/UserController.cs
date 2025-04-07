using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.DataAccess;
using RegistrationAPI.Models;

namespace RegistrationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] User user)
        {
            var response = await _userRepository.CreateUserAsync(user);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(UsersResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<UsersResponse>> GetAllUsers()
        {
            var response = await _userRepository.GetAllUsersAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var response = await _userRepository.GetUserByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id, [FromBody] User user)
        {
            var response = await _userRepository.UpdateUserAsync(id, user);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> DeleteUser(int id)
        {
            var response = await _userRepository.DeleteUserAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
} 