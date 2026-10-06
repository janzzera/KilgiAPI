using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProviderPaymentsController : BaseApiController
    {
        public ProviderPaymentsController(AppDbContext context) : base(context)
        {
        }

        // GET: api/ProviderPayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderPayment>>> GetProviderPayment()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.ProviderPayments.Where(e => e.UserId == userId).ToListAsync();
        }

        // GET: api/ProviderPayments/5
        [HttpGet("{paymentid}")]
        public async Task<ActionResult<ProviderPayment>> GetProviderPayment(string paymentid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var providerpayment = await _context.ProviderPayments
                .FirstOrDefaultAsync(e => e.PaymentId == paymentid && e.UserId == userId);

            if (providerpayment == null)
            {
                return NotFound();
            }

            return providerpayment;
        }

        // PUT: api/ProviderPayments/5
        [HttpPut("{paymentid}")]
        public async Task<IActionResult> PutProviderPayment(string? paymentid, ProviderPayment providerpayment)
        {
            if (paymentid != providerpayment.PaymentId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.ProviderPayments
                .AnyAsync(e => e.PaymentId == paymentid && e.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            var providerBelongsToUser = await _context.Providers
                .AnyAsync(p => p.ProviderId == providerpayment.ProviderId && p.UserId == userId);
            if (!providerBelongsToUser)
            {
                return BadRequest("The specified Provider does not exist or does not belong to the current user.");
            }

            providerpayment.UserId = userId;
            _context.Entry(providerpayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderPaymentExists(paymentid, userId))
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

        // POST: api/ProviderPayments
        [HttpPost]
        public async Task<ActionResult<ProviderPayment>> PostProviderPayment(ProviderPayment providerpayment)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var providerBelongsToUser = await _context.Providers
                .AnyAsync(p => p.ProviderId == providerpayment.ProviderId && p.UserId == userId);
            if (!providerBelongsToUser)
            {
                return BadRequest("The specified Provider does not exist or does not belong to the current user.");
            }

            providerpayment.UserId = userId;
            _context.ProviderPayments.Add(providerpayment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProviderPayment", new { paymentid = providerpayment.PaymentId }, providerpayment);
        }

        // DELETE: api/ProviderPayments/5
        [HttpDelete("{paymentid}")]
        public async Task<IActionResult> DeleteProviderPayment(string? paymentid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var providerpayment = await _context.ProviderPayments
                .FirstOrDefaultAsync(e => e.PaymentId == paymentid && e.UserId == userId);
            if (providerpayment == null)
            {
                return NotFound();
            }

            _context.ProviderPayments.Remove(providerpayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProviderPaymentExists(string? paymentid, string userId)
        {
            return _context.ProviderPayments.Any(e => e.PaymentId == paymentid && e.UserId == userId);
        }
    }
}

