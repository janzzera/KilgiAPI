using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class SpoilageLogsController : BaseApiController
    {
        public SpoilageLogsController(AppDbContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpoilageLog>>> GetSpoilageLog()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.SpoilageLogs.Where(s => s.Lot.UserId == userId).ToListAsync();
        }

        [HttpGet("{logId}")]
        public async Task<ActionResult<SpoilageLog>> GetSpoilageLog(string logId)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var spoilageLog = await _context.SpoilageLogs
                .FirstOrDefaultAsync(s => s.LogId == logId && s.Lot.UserId == userId);

            if (spoilageLog == null)
            {
                return NotFound();
            }

            return spoilageLog;
        }

        [HttpPut("{logId}")]
        public async Task<IActionResult> PutSpoilageLog(string? logId, SpoilageLog spoilageLog)
        {
            if (logId != spoilageLog.LogId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.SpoilageLogs
                .AnyAsync(s => s.LogId == logId && s.Lot.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            var lotBelongsToUser = await _context.Lots
                .AnyAsync(l => l.LotId == spoilageLog.LotId && l.UserId == userId);
            if (!lotBelongsToUser)
            {
                return BadRequest("The specified Lot does not exist or does not belong to the current user.");
            }

            _context.Entry(spoilageLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogExists(logId, userId))
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

        [HttpPost]
        public async Task<ActionResult<SpoilageLog>> PostSpoilageLog(SpoilageLog spoilageLog)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var lotBelongsToUser = await _context.Lots
                .AnyAsync(l => l.LotId == spoilageLog.LotId && l.UserId == userId);
            if (!lotBelongsToUser)
            {
                return BadRequest("The specified Lot does not exist or does not belong to the current user.");
            }

            _context.SpoilageLogs.Add(spoilageLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpoilageLog", new { logId = spoilageLog.LogId }, spoilageLog);
        }

        [HttpDelete("{logId}")]
        public async Task<IActionResult> DeleteSpoilageLog(string? logId)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var spoilageLog = await _context.SpoilageLogs
                .FirstOrDefaultAsync(s => s.LogId == logId && s.Lot.UserId == userId);
            if (spoilageLog == null)
            {
                return NotFound();
            }

            _context.SpoilageLogs.Remove(spoilageLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogExists(string logId, string userId)
        {
            return _context.SpoilageLogs.Any(log => log.LogId == logId && log.Lot.UserId == userId);
        }
    }
}

