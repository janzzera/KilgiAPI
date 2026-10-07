using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : BaseApiController
    {
        public UsersController(AppDbContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.Users.Where(user => user.UserId == userId).ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(string userId)
        {
            var currentUserId = CurrentUserId;
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            if (userId != currentUserId)
            {
                return Forbid();
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string? userId, User user)
        {
            if (user.UserId != userId)
                return BadRequest();

            var currentUserId = CurrentUserId;
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            if (userId != currentUserId)
            {
                return Forbid();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (user == null)
                return BadRequest();
            AuthController.CreatePasswordHash(user.PasswordHash, out string passwordHash, out string passwordSalt);

            var newUser = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Username = user.Username,
                DisplayName = user.DisplayName,
                BusinessName = user.BusinessName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                AccountStatus = "Active",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()

            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { userId = newUser.UserId }, newUser);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string? userId)
        {
            var currentUserId = CurrentUserId;
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            if (userId != currentUserId)
            {
                return Forbid();
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string? userId)
        {
            return _context.Users.Any(user => user.UserId == userId);
        }
    }
}

