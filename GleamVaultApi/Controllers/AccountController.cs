using GleamVaultApi.DB;
using GleamVaultApi.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace GleamVaultApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly GleamVaultContext _context;

        public AccountController(GleamVaultContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { error = "Username and password are required" });
            }

            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

            if (user == null)
            {
                return Unauthorized(new { error = "Invalid username or password" });
            }


            if (user.PasswordHash != request.Password)
            {
                return Unauthorized(new { error = "Invalid username or password" });
            }


            return Ok(new LoginResponse
            {
                ApiKey = user.ApiKeyHash,
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role.ToString(),
                Message = "Login successful"
            });
        }


    }
}