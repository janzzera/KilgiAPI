using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class CustomerPaymentAllocationsController : ControllerBase
{
    private readonly AppDbContext _context;
    public CustomerPaymentAllocationsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/CustomerPaymentAllocation
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerPaymentAllocation>>> GetCustomerPaymentAllocation()
    {
        return await _context.CustomerPaymentAllocations.ToListAsync();
    }

    // GET: api/CustomerPaymentAllocation/5
    [HttpGet("{allocationid}")]
    public async Task<ActionResult<CustomerPaymentAllocation>> GetCustomerPaymentAllocation(string allocationid)
    {
        var customerpaymentallocation = await _context.CustomerPaymentAllocations.FindAsync(allocationid);

        if (customerpaymentallocation == null)
        {
            return NotFound();
        }

        return customerpaymentallocation;
    }

    // PUT: api/CustomerPaymentAllocation/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{allocationid}")]
    public async Task<IActionResult> PutCustomerPaymentAllocation(string? allocationid, CustomerPaymentAllocation customerpaymentallocation)
    {
        if (allocationid != customerpaymentallocation.AllocationId)
        {
            return BadRequest();
        }

        _context.Entry(customerpaymentallocation).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerPaymentAllocationExists(allocationid))
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

    // POST: api/CustomerPaymentAllocation
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CustomerPaymentAllocation>> PostCustomerPaymentAllocation(CustomerPaymentAllocation customerpaymentallocation)
    {
        _context.CustomerPaymentAllocations.Add(customerpaymentallocation);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCustomerPaymentAllocation", new { allocationid = customerpaymentallocation.AllocationId }, customerpaymentallocation);
    }

    // DELETE: api/CustomerPaymentAllocation/5
    [HttpDelete("{allocationid}")]
    public async Task<IActionResult> DeleteCustomerPaymentAllocation(string? allocationid)
    {
        var customerpaymentallocation = await _context.CustomerPaymentAllocations.FindAsync(allocationid);
        if (customerpaymentallocation == null)
        {
            return NotFound();
        }

        _context.CustomerPaymentAllocations.Remove(customerpaymentallocation);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CustomerPaymentAllocationExists(string? allocationid)
    {
        return _context.CustomerPaymentAllocations.Any(e => e.AllocationId == allocationid);
    }
}
