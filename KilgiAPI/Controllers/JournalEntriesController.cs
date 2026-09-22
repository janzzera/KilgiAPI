using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class JournalEntriesController : ControllerBase
{
    private readonly AppDbContext _context;
    public JournalEntriesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/JournalEntry
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JournalEntry>>> GetJournalEntry()
    {
        return await _context.JournalEntries.ToListAsync();
    }

    // GET: api/JournalEntry/5
    [HttpGet("{entryid}")]
    public async Task<ActionResult<JournalEntry>> GetJournalEntry(string entryid)
    {
        var journalentry = await _context.JournalEntries.FindAsync(entryid);

        if (journalentry == null)
        {
            return NotFound();
        }

        return journalentry;
    }

    // PUT: api/JournalEntry/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{entryid}")]
    public async Task<IActionResult> PutJournalEntry(string? entryid, JournalEntry journalentry)
    {
        if (entryid != journalentry.EntryId)
        {
            return BadRequest();
        }

        _context.Entry(journalentry).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JournalEntryExists(entryid))
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

    // POST: api/JournalEntry
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<JournalEntry>> PostJournalEntry(JournalEntry journalentry)
    {
        _context.JournalEntries.Add(journalentry);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetJournalEntry", new { entryid = journalentry.EntryId }, journalentry);
    }

    // DELETE: api/JournalEntry/5
    [HttpDelete("{entryid}")]
    public async Task<IActionResult> DeleteJournalEntry(string? entryid)
    {
        var journalentry = await _context.JournalEntries.FindAsync(entryid);
        if (journalentry == null)
        {
            return NotFound();
        }

        _context.JournalEntries.Remove(journalentry);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool JournalEntryExists(string? entryid)
    {
        return _context.JournalEntries.Any(e => e.EntryId == entryid);
    }
}
