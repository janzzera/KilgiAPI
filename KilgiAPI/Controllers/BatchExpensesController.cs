using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class BatchExpensesController : BaseApiController
    {
        public BatchExpensesController(AppDbContext context) : base(context)
        {
        }

        // GET: api/BatchExpenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BatchExpense>>> GetBatchExpense()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.BatchExpenses.Where(e => e.Lot.UserId == userId).ToListAsync();
        }

        // GET: api/BatchExpenses/5
        [HttpGet("{expenseid}")]
        public async Task<ActionResult<BatchExpense>> GetBatchExpense(string expenseid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var batchexpense = await _context.BatchExpenses
                .FirstOrDefaultAsync(e => e.ExpenseId == expenseid && e.Lot.UserId == userId);

            if (batchexpense == null)
            {
                return NotFound();
            }

            return batchexpense;
        }

        // PUT: api/BatchExpenses/5
        [HttpPut("{expenseid}")]
        public async Task<IActionResult> PutBatchExpense(string? expenseid, BatchExpense batchexpense)
        {
            if (expenseid != batchexpense.ExpenseId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.BatchExpenses.AnyAsync(e => e.ExpenseId == expenseid && e.Lot.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            var lotExists = await _context.Lots.AnyAsync(l => l.LotId == batchexpense.LotId && l.UserId == userId);
            if (!lotExists)
            {
                return BadRequest("The specified Lot does not exist or does not belong to the current user.");
            }

            _context.Entry(batchexpense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchExpenseExists(expenseid, userId))
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

        // POST: api/BatchExpenses
        [HttpPost]
        public async Task<ActionResult<BatchExpense>> PostBatchExpense(BatchExpense batchexpense)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var lotExists = await _context.Lots.AnyAsync(l => l.LotId == batchexpense.LotId && l.UserId == userId);
            if (!lotExists)
            {
                return BadRequest("The specified Lot does not exist or does not belong to the current user.");
            }

            _context.BatchExpenses.Add(batchexpense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBatchExpense", new { expenseid = batchexpense.ExpenseId }, batchexpense);
        }

        // DELETE: api/BatchExpenses/5
        [HttpDelete("{expenseid}")]
        public async Task<IActionResult> DeleteBatchExpense(string? expenseid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var batchexpense = await _context.BatchExpenses
                .FirstOrDefaultAsync(e => e.ExpenseId == expenseid && e.Lot.UserId == userId);
            if (batchexpense == null)
            {
                return NotFound();
            }

            _context.BatchExpenses.Remove(batchexpense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BatchExpenseExists(string? expenseid, string userId)
        {
            return _context.BatchExpenses.Any(e => e.ExpenseId == expenseid && e.Lot.UserId == userId);
        }
    }
}

