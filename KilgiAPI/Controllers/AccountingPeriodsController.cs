using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountingPeriodsController : BaseApiController
    {
        public AccountingPeriodsController(AppDbContext context) : base(context)
        {
        }

        // GET: api/AccountingPeriods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountingPeriod>>> GetAccountingPeriod()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.AccountingPeriods.Where(e => e.UserId == userId).ToListAsync();
        }

        // GET: api/AccountingPeriods/5
        [HttpGet("{periodid}")]
        public async Task<ActionResult<AccountingPeriod>> GetAccountingPeriod(long periodid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var accountingperiod = await _context.AccountingPeriods
                .FirstOrDefaultAsync(e => e.PeriodId == periodid && e.UserId == userId);

            if (accountingperiod == null)
            {
                return NotFound();
            }

            return accountingperiod;
        }

        // PUT: api/AccountingPeriods/5
        [HttpPut("{periodid}")]
        public async Task<IActionResult> PutAccountingPeriod(long? periodid, AccountingPeriod accountingperiod)
        {
            if (periodid != accountingperiod.PeriodId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.AccountingPeriods.AnyAsync(e => e.PeriodId == periodid && e.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            accountingperiod.UserId = userId;
            _context.Entry(accountingperiod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountingPeriodExists(periodid, userId))
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

        // POST: api/AccountingPeriods
        [HttpPost]
        public async Task<ActionResult<AccountingPeriod>> PostAccountingPeriod(AccountingPeriod accountingperiod)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            accountingperiod.UserId = userId;
            _context.AccountingPeriods.Add(accountingperiod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountingPeriod", new { periodid = accountingperiod.PeriodId }, accountingperiod);
        }

        // DELETE: api/AccountingPeriods/5
        [HttpDelete("{periodid}")]
        public async Task<IActionResult> DeleteAccountingPeriod(long? periodid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var accountingperiod = await _context.AccountingPeriods
                .FirstOrDefaultAsync(e => e.PeriodId == periodid && e.UserId == userId);
            if (accountingperiod == null)
            {
                return NotFound();
            }

            _context.AccountingPeriods.Remove(accountingperiod);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountingPeriodExists(long? periodid, string userId)
        {
            return _context.AccountingPeriods.Any(e => e.PeriodId == periodid && e.UserId == userId);
        }
    }
}

