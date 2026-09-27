using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpoilageLogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SpoilageLogsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpoilageLog>>> GetSpoilageLog()
        {
            return await _context.SpoilageLogs.ToListAsync();
        }

        [HttpGet("{logId}")]
        public async Task<ActionResult<SpoilageLog>> GetSpoilageLog(string logId)
        {
            var spoilageLog = await _context.SpoilageLogs.FindAsync(logId);

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

            _context.Entry(spoilageLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!LogExists(logId))
                {
                    return NotFound();
                }
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<SpoilageLog>> PostSpoilageLog(SpoilageLog spoilageLog)
        {
            _context.SpoilageLogs.Add(spoilageLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpoilageLog", new {logId =  spoilageLog.LogId}, spoilageLog);
        }

        [HttpDelete("{logId}")]
        public async Task<IActionResult> DeleteSpoilageLog(string? logId)
        {
            var spoilageLog = await _context.SpoilageLogs.FindAsync(logId);
            if (spoilageLog == null)
            {
                return NotFound();
            }

            _context.SpoilageLogs.Remove(spoilageLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogExists(string logId) {
            return _context.SpoilageLogs.Any(log => log.LogId == logId);
        }
    }
}
