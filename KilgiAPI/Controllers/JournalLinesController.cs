using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class JournalLinesController : ControllerBase
{
    private readonly AppDbContext _context;
    public JournalLinesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/JournalLine
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JournalLine>>> GetJournalLine()
    {
        return await _context.JournalLines.ToListAsync();
    }

    // GET: api/JournalLine/5
    [HttpGet("{lineid}")]
    public async Task<ActionResult<JournalLine>> GetJournalLine(string lineid)
    {
        var journalline = await _context.JournalLines.FindAsync(lineid);

        if (journalline == null)
        {
            return NotFound();
        }

        return journalline;
    }

    // PUT: api/JournalLine/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{lineid}")]
    public async Task<IActionResult> PutJournalLine(string? lineid, JournalLine journalline)
    {
        if (lineid != journalline.LineId)
        {
            return BadRequest();
        }

        _context.Entry(journalline).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JournalLineExists(lineid))
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

    // POST: api/JournalLine
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<JournalLine>> PostJournalLine(JournalLine journalline)
    {
        _context.JournalLines.Add(journalline);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetJournalLine", new { lineid = journalline.LineId }, journalline);
    }

    // DELETE: api/JournalLine/5
    [HttpDelete("{lineid}")]
    public async Task<IActionResult> DeleteJournalLine(string? lineid)
    {
        var journalline = await _context.JournalLines.FindAsync(lineid);
        if (journalline == null)
        {
            return NotFound();
        }

        _context.JournalLines.Remove(journalline);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool JournalLineExists(string? lineid)
    {
        return _context.JournalLines.Any(e => e.LineId == lineid);
    }
}
