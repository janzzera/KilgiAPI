using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class RetailSalesController : ControllerBase
{
    private readonly AppDbContext _context;
    public RetailSalesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/RetailSale
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RetailSale>>> GetRetailSale()
    {
        return await _context.RetailSales.ToListAsync();
    }

    // GET: api/RetailSale/5
    [HttpGet("{saleid}")]
    public async Task<ActionResult<RetailSale>> GetRetailSale(string saleid)
    {
        var retailsale = await _context.RetailSales.FindAsync(saleid);

        if (retailsale == null)
        {
            return NotFound();
        }

        return retailsale;
    }

    // PUT: api/RetailSale/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{saleid}")]
    public async Task<IActionResult> PutRetailSale(string? saleid, RetailSale retailsale)
    {
        if (saleid != retailsale.SaleId)
        {
            return BadRequest();
        }

        _context.Entry(retailsale).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RetailSaleExists(saleid))
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

    // POST: api/RetailSale
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<RetailSale>> PostRetailSale(RetailSale retailsale)
    {
        _context.RetailSales.Add(retailsale);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetRetailSale", new { saleid = retailsale.SaleId }, retailsale);
    }

    // DELETE: api/RetailSale/5
    [HttpDelete("{saleid}")]
    public async Task<IActionResult> DeleteRetailSale(string? saleid)
    {
        var retailsale = await _context.RetailSales.FindAsync(saleid);
        if (retailsale == null)
        {
            return NotFound();
        }

        _context.RetailSales.Remove(retailsale);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RetailSaleExists(string? saleid)
    {
        return _context.RetailSales.Any(e => e.SaleId == saleid);
    }
}
