using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class JournalEntriesController : BaseApiController
    {
        public JournalEntriesController(AppDbContext context) : base(context)
        {
        }

        // GET: api/JournalEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalEntry>>> GetJournalEntry()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.JournalEntries.Where(e => e.UserId == userId).ToListAsync();
        }

        // GET: api/JournalEntries/5
        [HttpGet("{entryid}")]
        public async Task<ActionResult<JournalEntry>> GetJournalEntry(string entryid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var journalentry = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.EntryId == entryid && e.UserId == userId);

            if (journalentry == null)
            {
                return NotFound();
            }

            return journalentry;
        }

        // PUT: api/JournalEntries/5
        [HttpPut("{entryid}")]
        public async Task<IActionResult> PutJournalEntry(string? entryid, JournalEntry journalentry)
        {
            if (entryid != journalentry.EntryId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.JournalEntries
                .AnyAsync(e => e.EntryId == entryid && e.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            journalentry.UserId = userId;
            _context.Entry(journalentry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalEntryExists(entryid, userId))
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

        // POST: api/JournalEntries
        [HttpPost]
        public async Task<ActionResult<JournalEntry>> PostJournalEntry(JournalEntry journalentry)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            journalentry.UserId = userId;
            _context.JournalEntries.Add(journalentry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJournalEntry", new { entryid = journalentry.EntryId }, journalentry);
        }

        // DELETE: api/JournalEntries/5
        [HttpDelete("{entryid}")]
        public async Task<IActionResult> DeleteJournalEntry(string? entryid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var journalentry = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.EntryId == entryid && e.UserId == userId);
            if (journalentry == null)
            {
                return NotFound();
            }

            _context.JournalEntries.Remove(journalentry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JournalEntryExists(string? entryid, string userId)
        {
            return _context.JournalEntries.Any(e => e.EntryId == entryid && e.UserId == userId);
        }
    }
}

