using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProviderPaymentsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProviderPaymentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/ProviderPayment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProviderPayment>>> GetProviderPayment()
    {
        return await _context.ProviderPayments.ToListAsync();
    }

    // GET: api/ProviderPayment/5
    [HttpGet("{paymentid}")]
    public async Task<ActionResult<ProviderPayment>> GetProviderPayment(string paymentid)
    {
        var providerpayment = await _context.ProviderPayments.FindAsync(paymentid);

        if (providerpayment == null)
        {
            return NotFound();
        }

        return providerpayment;
    }

    // PUT: api/ProviderPayment/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{paymentid}")]
    public async Task<IActionResult> PutProviderPayment(string? paymentid, ProviderPayment providerpayment)
    {
        if (paymentid != providerpayment.PaymentId)
        {
            return BadRequest();
        }

        _context.Entry(providerpayment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProviderPaymentExists(paymentid))
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

    // POST: api/ProviderPayment
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ProviderPayment>> PostProviderPayment(ProviderPayment providerpayment)
    {
        _context.ProviderPayments.Add(providerpayment);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProviderPayment", new { paymentid = providerpayment.PaymentId }, providerpayment);
    }

    // DELETE: api/ProviderPayment/5
    [HttpDelete("{paymentid}")]
    public async Task<IActionResult> DeleteProviderPayment(string? paymentid)
    {
        var providerpayment = await _context.ProviderPayments.FindAsync(paymentid);
        if (providerpayment == null)
        {
            return NotFound();
        }

        _context.ProviderPayments.Remove(providerpayment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProviderPaymentExists(string? paymentid)
    {
        return _context.ProviderPayments.Any(e => e.PaymentId == paymentid);
    }
}
