using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Data;
using KilgiAPI.Models;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context = new AppDbContext();
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(String userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(String? userId, User user)
        {
            if (user.UserId != userId)
                return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent(); ;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new {userId = user.UserId}, user);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(String? userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(String userId)
        {
            return _context.Users.Any(user => user.UserId == userId);
        }
    }
}
