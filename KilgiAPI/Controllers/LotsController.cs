using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LotsController : ControllerBase
{
    private readonly AppDbContext _context;
    public LotsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Lot
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lot>>> GetLot()
    {
        return await _context.Lots.ToListAsync();
    }

    // GET: api/Lot/5
    [HttpGet("{lotid}")]
    public async Task<ActionResult<Lot>> GetLot(string lotid)
    {
        var lot = await _context.Lots.FindAsync(lotid);

        if (lot == null)
        {
            return NotFound();
        }

        return lot;
    }

    // PUT: api/Lot/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{lotid}")]
    public async Task<IActionResult> PutLot(string? lotid, Lot lot)
    {
        if (lotid != lot.LotId)
        {
            return BadRequest();
        }

        _context.Entry(lot).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LotExists(lotid))
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

    // POST: api/Lot
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Lot>> PostLot(Lot lot)
    {
        _context.Lots.Add(lot);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLot", new { lotid = lot.LotId }, lot);
    }

    // DELETE: api/Lot/5
    [HttpDelete("{lotid}")]
    public async Task<IActionResult> DeleteLot(string? lotid)
    {
        var lot = await _context.Lots.FindAsync(lotid);
        if (lot == null)
        {
            return NotFound();
        }

        _context.Lots.Remove(lot);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LotExists(string? lotid)
    {
        return _context.Lots.Any(e => e.LotId == lotid);
    }
}
