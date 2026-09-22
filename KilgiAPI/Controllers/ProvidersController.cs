using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class ProvidersController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProvidersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Provider
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Provider>>> GetProvider()
    {
        return await _context.Providers.ToListAsync();
    }

    // GET: api/Provider/5
    [HttpGet("{providerid}")]
    public async Task<ActionResult<Provider>> GetProvider(string providerid)
    {
        var provider = await _context.Providers.FindAsync(providerid);

        if (provider == null)
        {
            return NotFound();
        }

        return provider;
    }

    // PUT: api/Provider/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{providerid}")]
    public async Task<IActionResult> PutProvider(string? providerid, Provider provider)
    {
        if (providerid != provider.ProviderId)
        {
            return BadRequest();
        }

        _context.Entry(provider).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProviderExists(providerid))
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

    // POST: api/Provider
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Provider>> PostProvider(Provider provider)
    {
        _context.Providers.Add(provider);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProvider", new { providerid = provider.ProviderId }, provider);
    }

    // DELETE: api/Provider/5
    [HttpDelete("{providerid}")]
    public async Task<IActionResult> DeleteProvider(string? providerid)
    {
        var provider = await _context.Providers.FindAsync(providerid);
        if (provider == null)
        {
            return NotFound();
        }

        _context.Providers.Remove(provider);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProviderExists(string? providerid)
    {
        return _context.Providers.Any(e => e.ProviderId == providerid);
    }
}
