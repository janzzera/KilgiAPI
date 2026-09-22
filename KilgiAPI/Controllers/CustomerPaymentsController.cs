using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class CustomerPaymentsController : ControllerBase
{
    private readonly AppDbContext _context;
    public CustomerPaymentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/CustomerPayment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerPayment>>> GetCustomerPayment()
    {
        return await _context.CustomerPayments.ToListAsync();
    }

    // GET: api/CustomerPayment/5
    [HttpGet("{paymentid}")]
    public async Task<ActionResult<CustomerPayment>> GetCustomerPayment(string paymentid)
    {
        var customerpayment = await _context.CustomerPayments.FindAsync(paymentid);

        if (customerpayment == null)
        {
            return NotFound();
        }

        return customerpayment;
    }

    // PUT: api/CustomerPayment/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{paymentid}")]
    public async Task<IActionResult> PutCustomerPayment(string? paymentid, CustomerPayment customerpayment)
    {
        if (paymentid != customerpayment.PaymentId)
        {
            return BadRequest();
        }

        _context.Entry(customerpayment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerPaymentExists(paymentid))
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

    // POST: api/CustomerPayment
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CustomerPayment>> PostCustomerPayment(CustomerPayment customerpayment)
    {
        _context.CustomerPayments.Add(customerpayment);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCustomerPayment", new { paymentid = customerpayment.PaymentId }, customerpayment);
    }

    // DELETE: api/CustomerPayment/5
    [HttpDelete("{paymentid}")]
    public async Task<IActionResult> DeleteCustomerPayment(string? paymentid)
    {
        var customerpayment = await _context.CustomerPayments.FindAsync(paymentid);
        if (customerpayment == null)
        {
            return NotFound();
        }

        _context.CustomerPayments.Remove(customerpayment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CustomerPaymentExists(string? paymentid)
    {
        return _context.CustomerPayments.Any(e => e.PaymentId == paymentid);
    }
}
