using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class JournalLinesController : BaseApiController
    {
        public JournalLinesController(AppDbContext context) : base(context)
        {
        }

        // GET: api/JournalLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalLine>>> GetJournalLine()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.JournalLines
                .Where(e => e.Entry.UserId == userId)
                .ToListAsync();
        }

        // GET: api/JournalLines/5
        [HttpGet("{lineid}")]
        public async Task<ActionResult<JournalLine>> GetJournalLine(string lineid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var journalline = await _context.JournalLines
                .FirstOrDefaultAsync(e => e.LineId == lineid && e.Entry.UserId == userId);

            if (journalline == null)
            {
                return NotFound();
            }

            return journalline;
        }

        // PUT: api/JournalLines/5
        [HttpPut("{lineid}")]
        public async Task<IActionResult> PutJournalLine(string? lineid, JournalLine journalline)
        {
            if (lineid != journalline.LineId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.JournalLines
                .AnyAsync(e => e.LineId == lineid && e.Entry.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            var entryBelongsToUser = await _context.JournalEntries
                .AnyAsync(e => e.EntryId == journalline.EntryId && e.UserId == userId);
            if (!entryBelongsToUser)
            {
                return BadRequest("The specified Journal Entry does not exist or does not belong to the current user.");
            }

            _context.Entry(journalline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalLineExists(lineid, userId))
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

        // POST: api/JournalLines
        [HttpPost]
        public async Task<ActionResult<JournalLine>> PostJournalLine(JournalLine journalline)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var entryBelongsToUser = await _context.JournalEntries
                .AnyAsync(e => e.EntryId == journalline.EntryId && e.UserId == userId);
            if (!entryBelongsToUser)
            {
                return BadRequest("The specified Journal Entry does not exist or does not belong to the current user.");
            }

            _context.JournalLines.Add(journalline);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJournalLine", new { lineid = journalline.LineId }, journalline);
        }

        // DELETE: api/JournalLines/5
        [HttpDelete("{lineid}")]
        public async Task<IActionResult> DeleteJournalLine(string? lineid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var journalline = await _context.JournalLines
                .FirstOrDefaultAsync(e => e.LineId == lineid && e.Entry.UserId == userId);
            if (journalline == null)
            {
                return NotFound();
            }

            _context.JournalLines.Remove(journalline);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JournalLineExists(string? lineid, string userId)
        {
            return _context.JournalLines.Any(e => e.LineId == lineid && e.Entry.UserId == userId);
        }
    }
}

