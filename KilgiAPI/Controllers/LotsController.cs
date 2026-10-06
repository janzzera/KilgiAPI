using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class LotsController : BaseApiController
    {
        public LotsController(AppDbContext context) : base(context)
        {
        }

        // GET: api/Lots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lot>>> GetLot()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.Lots.Where(e => e.UserId == userId).ToListAsync();
        }

        // GET: api/Lots/5
        [HttpGet("{lotid}")]
        public async Task<ActionResult<Lot>> GetLot(string lotid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var lot = await _context.Lots
                .FirstOrDefaultAsync(e => e.LotId == lotid && e.UserId == userId);

            if (lot == null)
            {
                return NotFound();
            }

            return lot;
        }

        // PUT: api/Lots/5
        [HttpPut("{lotid}")]
        public async Task<IActionResult> PutLot(string? lotid, Lot lot)
        {
            if (lotid != lot.LotId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.Lots
                .AnyAsync(e => e.LotId == lotid && e.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            var providerBelongsToUser = await _context.Providers
                .AnyAsync(p => p.ProviderId == lot.ProviderId && p.UserId == userId);
            if (!providerBelongsToUser)
            {
                return BadRequest("The specified Provider does not exist or does not belong to the current user.");
            }

            lot.UserId = userId;
            _context.Entry(lot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LotExists(lotid, userId))
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

        // POST: api/Lots
        [HttpPost]
        public async Task<ActionResult<Lot>> PostLot(Lot lot)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var providerBelongsToUser = await _context.Providers
                .AnyAsync(p => p.ProviderId == lot.ProviderId && p.UserId == userId);
            if (!providerBelongsToUser)
            {
                return BadRequest("The specified Provider does not exist or does not belong to the current user.");
            }

            lot.UserId = userId;
            _context.Lots.Add(lot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLot", new { lotid = lot.LotId }, lot);
        }

        // DELETE: api/Lots/5
        [HttpDelete("{lotid}")]
        public async Task<IActionResult> DeleteLot(string? lotid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var lot = await _context.Lots
                .FirstOrDefaultAsync(e => e.LotId == lotid && e.UserId == userId);
            if (lot == null)
            {
                return NotFound();
            }

            _context.Lots.Remove(lot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LotExists(string? lotid, string userId)
        {
            return _context.Lots.Any(e => e.LotId == lotid && e.UserId == userId);
        }
    }
}

