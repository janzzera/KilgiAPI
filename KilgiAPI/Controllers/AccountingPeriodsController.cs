using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountingPeriodsController : ControllerBase
{
    private readonly AppDbContext _context;
    public AccountingPeriodsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/AccountingPeriod
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountingPeriod>>> GetAccountingPeriod()
    {
        return await _context.AccountingPeriods.ToListAsync();
    }

    // GET: api/AccountingPeriod/5
    [HttpGet("{periodid}")]
    public async Task<ActionResult<AccountingPeriod>> GetAccountingPeriod(long periodid)
    {
        var accountingperiod = await _context.AccountingPeriods.FindAsync(periodid);

        if (accountingperiod == null)
        {
            return NotFound();
        }

        return accountingperiod;
    }

    // PUT: api/AccountingPeriod/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{periodid}")]
    public async Task<IActionResult> PutAccountingPeriod(long? periodid, AccountingPeriod accountingperiod)
    {
        if (periodid != accountingperiod.PeriodId)
        {
            return BadRequest();
        }

        _context.Entry(accountingperiod).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AccountingPeriodExists(periodid))
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

    // POST: api/AccountingPeriod
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<AccountingPeriod>> PostAccountingPeriod(AccountingPeriod accountingperiod)
    {
        _context.AccountingPeriods.Add(accountingperiod);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAccountingPeriod", new { periodid = accountingperiod.PeriodId }, accountingperiod);
    }

    // DELETE: api/AccountingPeriod/5
    [HttpDelete("{periodid}")]
    public async Task<IActionResult> DeleteAccountingPeriod(long? periodid)
    {
        var accountingperiod = await _context.AccountingPeriods.FindAsync(periodid);
        if (accountingperiod == null)
        {
            return NotFound();
        }

        _context.AccountingPeriods.Remove(accountingperiod);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AccountingPeriodExists(long? periodid)
    {
        return _context.AccountingPeriods.Any(e => e.PeriodId == periodid);
    }
}
