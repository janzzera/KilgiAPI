using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProvidersController : BaseApiController
    {
        public ProvidersController(AppDbContext context) : base(context)
        {
        }

        // GET: api/Providers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProvider()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.Providers.Where(e => e.UserId == userId).ToListAsync();
        }

        // GET: api/Providers/5
        [HttpGet("{providerid}")]
        public async Task<ActionResult<Provider>> GetProvider(string providerid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var provider = await _context.Providers
                .FirstOrDefaultAsync(e => e.ProviderId == providerid && e.UserId == userId);

            if (provider == null)
            {
                return NotFound();
            }

            return provider;
        }

        // PUT: api/Providers/5
        [HttpPut("{providerid}")]
        public async Task<IActionResult> PutProvider(string? providerid, Provider provider)
        {
            if (providerid != provider.ProviderId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.Providers
                .AnyAsync(e => e.ProviderId == providerid && e.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            provider.UserId = userId;
            _context.Entry(provider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderExists(providerid, userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Providers
        [HttpPost]
        public async Task<ActionResult<Provider>> PostProvider(Provider provider)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            provider.UserId = userId;
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProvider", new { providerid = provider.ProviderId }, provider);
        }

        // DELETE: api/Providers/5
        [HttpDelete("{providerid}")]
        public async Task<IActionResult> DeleteProvider(string? providerid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var provider = await _context.Providers
                .FirstOrDefaultAsync(e => e.ProviderId == providerid && e.UserId == userId);
            if (provider == null)
            {
                return NotFound();
            }

            _context.Providers.Remove(provider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProviderExists(string? providerid, string userId)
        {
            return _context.Providers.Any(e => e.ProviderId == providerid && e.UserId == userId);
        }
    }
}

