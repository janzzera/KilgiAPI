using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class CustomerPaymentsController : BaseApiController
    {
        public CustomerPaymentsController(AppDbContext context) : base(context)
        {
        }

        // GET: api/CustomerPayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerPayment>>> GetCustomerPayment()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.CustomerPayments.Where(e => e.UserId == userId).ToListAsync();
        }

        // GET: api/CustomerPayments/5
        [HttpGet("{paymentid}")]
        public async Task<ActionResult<CustomerPayment>> GetCustomerPayment(string paymentid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customerpayment = await _context.CustomerPayments
                .FirstOrDefaultAsync(e => e.PaymentId == paymentid && e.UserId == userId);

            if (customerpayment == null)
            {
                return NotFound();
            }

            return customerpayment;
        }

        // PUT: api/CustomerPayments/5
        [HttpPut("{paymentid}")]
        public async Task<IActionResult> PutCustomerPayment(string? paymentid, CustomerPayment customerpayment)
        {
            if (paymentid != customerpayment.PaymentId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.CustomerPayments
                .AnyAsync(e => e.PaymentId == paymentid && e.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            var customerBelongsToUser = await _context.Customers
                .AnyAsync(c => c.CustomerId == customerpayment.CustomerId && c.UserId == userId);
            if (!customerBelongsToUser)
            {
                return BadRequest("The specified Customer does not exist or does not belong to the current user.");
            }

            customerpayment.UserId = userId;
            _context.Entry(customerpayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerPaymentExists(paymentid, userId))
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

        // POST: api/CustomerPayments
        [HttpPost]
        public async Task<ActionResult<CustomerPayment>> PostCustomerPayment(CustomerPayment customerpayment)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customerBelongsToUser = await _context.Customers
                .AnyAsync(c => c.CustomerId == customerpayment.CustomerId && c.UserId == userId);
            if (!customerBelongsToUser)
            {
                return BadRequest("The specified Customer does not exist or does not belong to the current user.");
            }

            customerpayment.UserId = userId;
            _context.CustomerPayments.Add(customerpayment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerPayment", new { paymentid = customerpayment.PaymentId }, customerpayment);
        }

        // DELETE: api/CustomerPayments/5
        [HttpDelete("{paymentid}")]
        public async Task<IActionResult> DeleteCustomerPayment(string? paymentid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customerpayment = await _context.CustomerPayments
                .FirstOrDefaultAsync(e => e.PaymentId == paymentid && e.UserId == userId);
            if (customerpayment == null)
            {
                return NotFound();
            }

            _context.CustomerPayments.Remove(customerpayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerPaymentExists(string? paymentid, string userId)
        {
            return _context.CustomerPayments.Any(e => e.PaymentId == paymentid && e.UserId == userId);
        }
    }
}

