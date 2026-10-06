using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProviderPaymentAllocationsController : BaseApiController
    {
        public ProviderPaymentAllocationsController(AppDbContext context) : base(context)
        {
        }

        // GET: api/ProviderPaymentAllocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderPaymentAllocation>>> GetProviderPaymentAllocation()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.ProviderPaymentAllocations
                .Where(e => e.Payment.UserId == userId)
                .ToListAsync();
        }

        // GET: api/ProviderPaymentAllocations/5
        [HttpGet("{allocationid}")]
        public async Task<ActionResult<ProviderPaymentAllocation>> GetProviderPaymentAllocation(string allocationid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var providerpaymentallocation = await _context.ProviderPaymentAllocations
                .FirstOrDefaultAsync(e => e.AllocationId == allocationid && e.Payment.UserId == userId);

            if (providerpaymentallocation == null)
            {
                return NotFound();
            }

            return providerpaymentallocation;
        }

        // PUT: api/ProviderPaymentAllocations/5
        [HttpPut("{allocationid}")]
        public async Task<IActionResult> PutProviderPaymentAllocation(string? allocationid, ProviderPaymentAllocation providerpaymentallocation)
        {
            if (allocationid != providerpaymentallocation.AllocationId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.ProviderPaymentAllocations
                .AnyAsync(e => e.AllocationId == allocationid && e.Payment.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            var paymentBelongsToUser = await _context.ProviderPayments
                .AnyAsync(p => p.PaymentId == providerpaymentallocation.PaymentId && p.UserId == userId);
            if (!paymentBelongsToUser)
            {
                return BadRequest("The specified Payment does not exist or does not belong to the current user.");
            }

            var lotBelongsToUser = await _context.Lots
                .AnyAsync(l => l.LotId == providerpaymentallocation.LotId && l.UserId == userId);
            if (!lotBelongsToUser)
            {
                return BadRequest("The specified Lot does not exist or does not belong to the current user.");
            }

            _context.Entry(providerpaymentallocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderPaymentAllocationExists(allocationid, userId))
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

        // POST: api/ProviderPaymentAllocations
        [HttpPost]
        public async Task<ActionResult<ProviderPaymentAllocation>> PostProviderPaymentAllocation(ProviderPaymentAllocation providerpaymentallocation)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var paymentBelongsToUser = await _context.ProviderPayments
                .AnyAsync(p => p.PaymentId == providerpaymentallocation.PaymentId && p.UserId == userId);
            if (!paymentBelongsToUser)
            {
                return BadRequest("The specified Payment does not exist or does not belong to the current user.");
            }

            var lotBelongsToUser = await _context.Lots
                .AnyAsync(l => l.LotId == providerpaymentallocation.LotId && l.UserId == userId);
            if (!lotBelongsToUser)
            {
                return BadRequest("The specified Lot does not exist or does not belong to the current user.");
            }

            _context.ProviderPaymentAllocations.Add(providerpaymentallocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProviderPaymentAllocation", new { allocationid = providerpaymentallocation.AllocationId }, providerpaymentallocation);
        }

        // DELETE: api/ProviderPaymentAllocations/5
        [HttpDelete("{allocationid}")]
        public async Task<IActionResult> DeleteProviderPaymentAllocation(string? allocationid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var providerpaymentallocation = await _context.ProviderPaymentAllocations
                .FirstOrDefaultAsync(e => e.AllocationId == allocationid && e.Payment.UserId == userId);
            if (providerpaymentallocation == null)
            {
                return NotFound();
            }

            _context.ProviderPaymentAllocations.Remove(providerpaymentallocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProviderPaymentAllocationExists(string? allocationid, string userId)
        {
            return _context.ProviderPaymentAllocations.Any(e => e.AllocationId == allocationid && e.Payment.UserId == userId);
        }
    }
}

