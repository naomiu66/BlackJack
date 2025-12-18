using Contracts.Dto.Requests;
using Contracts.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
using UserService.Data.Models;
using UserService.Services.Abstractions;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDtoResponse>> GetUserById(Guid id)
        {
            var user = await _usersService.GetByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new UserDtoResponse 
            {
                Id = user.Id,
                TelegramId = user.TelegramId,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Login = user.Login,
                Balance = user.Balance,
                TotalGames = user.TotalGames,
                TotalWins = user.TotalWins,
                TotalLosses = user.TotalLosses
            });
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<UserDtoResponse>>> GetAllUsers()
        {
            var users = await _usersService.GetAllAsync();
            return Ok(users.Select(u => new UserDtoResponse 
            {
                Id = u.Id,
                TelegramId = u.TelegramId,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Login = u.Login,
                Balance = u.Balance,
                TotalGames = u.TotalGames,
                TotalWins = u.TotalWins,
                TotalLosses = u.TotalLosses
            }).ToList());
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDtoRequest request)
        {
            var user = new User
            {
                TelegramId = request.TelegramId,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Login = request.Login
            };

            var result = await _usersService.CreateAsync(user);

            if (!result)
            {
                return BadRequest("Failed to create user.");
            }
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDtoRequest request)
        {
            var user = await _usersService.GetByIdAsync(request.Id.ToString());

            if (user == null) return NotFound();

            user.TelegramId = request.TelegramId ?? user.TelegramId;
            user.Username = request.Username ?? user.Username;
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Login = request.Login ?? user.Login;
            user.Balance = request.Balance;
            user.TotalGames = request.TotalGames;
            user.TotalWins = request.TotalWins;
            user.TotalLosses = request.TotalLosses;

            var result = await _usersService.UpdateAsync(user);
            if (!result)
            {
                return BadRequest("Failed to update user.");
            }
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _usersService.DeleteAsync(id.ToString());
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
