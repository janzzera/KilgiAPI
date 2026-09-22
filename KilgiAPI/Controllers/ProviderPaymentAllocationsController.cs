using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class ProviderPaymentAllocationsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProviderPaymentAllocationsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/ProviderPaymentAllocation
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProviderPaymentAllocation>>> GetProviderPaymentAllocation()
    {
        return await _context.ProviderPaymentAllocations.ToListAsync();
    }

    // GET: api/ProviderPaymentAllocation/5
    [HttpGet("{allocationid}")]
    public async Task<ActionResult<ProviderPaymentAllocation>> GetProviderPaymentAllocation(string allocationid)
    {
        var providerpaymentallocation = await _context.ProviderPaymentAllocations.FindAsync(allocationid);

        if (providerpaymentallocation == null)
        {
            return NotFound();
        }

        return providerpaymentallocation;
    }

    // PUT: api/ProviderPaymentAllocation/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{allocationid}")]
    public async Task<IActionResult> PutProviderPaymentAllocation(string? allocationid, ProviderPaymentAllocation providerpaymentallocation)
    {
        if (allocationid != providerpaymentallocation.AllocationId)
        {
            return BadRequest();
        }

        _context.Entry(providerpaymentallocation).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProviderPaymentAllocationExists(allocationid))
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

    // POST: api/ProviderPaymentAllocation
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ProviderPaymentAllocation>> PostProviderPaymentAllocation(ProviderPaymentAllocation providerpaymentallocation)
    {
        _context.ProviderPaymentAllocations.Add(providerpaymentallocation);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProviderPaymentAllocation", new { allocationid = providerpaymentallocation.AllocationId }, providerpaymentallocation);
    }

    // DELETE: api/ProviderPaymentAllocation/5
    [HttpDelete("{allocationid}")]
    public async Task<IActionResult> DeleteProviderPaymentAllocation(string? allocationid)
    {
        var providerpaymentallocation = await _context.ProviderPaymentAllocations.FindAsync(allocationid);
        if (providerpaymentallocation == null)
        {
            return NotFound();
        }

        _context.ProviderPaymentAllocations.Remove(providerpaymentallocation);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProviderPaymentAllocationExists(string? allocationid)
    {
        return _context.ProviderPaymentAllocations.Any(e => e.AllocationId == allocationid);
    }
}
