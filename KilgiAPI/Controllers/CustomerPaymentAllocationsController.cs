using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class CustomerPaymentAllocationsController : BaseApiController
    {
        public CustomerPaymentAllocationsController(AppDbContext context) : base(context)
        {
        }

        // GET: api/CustomerPaymentAllocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerPaymentAllocation>>> GetCustomerPaymentAllocation()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.CustomerPaymentAllocations
                .Where(e => e.Payment.UserId == userId)
                .ToListAsync();
        }

        // GET: api/CustomerPaymentAllocations/5
        [HttpGet("{allocationid}")]
        public async Task<ActionResult<CustomerPaymentAllocation>> GetCustomerPaymentAllocation(string allocationid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customerpaymentallocation = await _context.CustomerPaymentAllocations
                .FirstOrDefaultAsync(e => e.AllocationId == allocationid && e.Payment.UserId == userId);

            if (customerpaymentallocation == null)
            {
                return NotFound();
            }

            return customerpaymentallocation;
        }

        // PUT: api/CustomerPaymentAllocations/5
        [HttpPut("{allocationid}")]
        public async Task<IActionResult> PutCustomerPaymentAllocation(string? allocationid, CustomerPaymentAllocation customerpaymentallocation)
        {
            if (allocationid != customerpaymentallocation.AllocationId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.CustomerPaymentAllocations
                .AnyAsync(e => e.AllocationId == allocationid && e.Payment.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            var paymentBelongsToUser = await _context.CustomerPayments
                .AnyAsync(p => p.PaymentId == customerpaymentallocation.PaymentId && p.UserId == userId);
            if (!paymentBelongsToUser)
            {
                return BadRequest("The specified Payment does not exist or does not belong to the current user.");
            }

            _context.Entry(customerpaymentallocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerPaymentAllocationExists(allocationid, userId))
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

        // POST: api/CustomerPaymentAllocations
        [HttpPost]
        public async Task<ActionResult<CustomerPaymentAllocation>> PostCustomerPaymentAllocation(CustomerPaymentAllocation customerpaymentallocation)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var paymentBelongsToUser = await _context.CustomerPayments
                .AnyAsync(p => p.PaymentId == customerpaymentallocation.PaymentId && p.UserId == userId);
            if (!paymentBelongsToUser)
            {
                return BadRequest("The specified Payment does not exist or does not belong to the current user.");
            }

            _context.CustomerPaymentAllocations.Add(customerpaymentallocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerPaymentAllocation", new { allocationid = customerpaymentallocation.AllocationId }, customerpaymentallocation);
        }

        // DELETE: api/CustomerPaymentAllocations/5
        [HttpDelete("{allocationid}")]
        public async Task<IActionResult> DeleteCustomerPaymentAllocation(string? allocationid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customerpaymentallocation = await _context.CustomerPaymentAllocations
                .FirstOrDefaultAsync(e => e.AllocationId == allocationid && e.Payment.UserId == userId);
            if (customerpaymentallocation == null)
            {
                return NotFound();
            }

            _context.CustomerPaymentAllocations.Remove(customerpaymentallocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerPaymentAllocationExists(string? allocationid, string userId)
        {
            return _context.CustomerPaymentAllocations.Any(e => e.AllocationId == allocationid && e.Payment.UserId == userId);
        }
    }
}

