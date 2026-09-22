using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KilgiAPI.Models;
using KilgiAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class BatchExpensesController : ControllerBase
{
    private readonly AppDbContext _context;
    public BatchExpensesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/BatchExpense
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BatchExpense>>> GetBatchExpense()
    {
        return await _context.BatchExpenses.ToListAsync();
    }

    // GET: api/BatchExpense/5
    [HttpGet("{expenseid}")]
    public async Task<ActionResult<BatchExpense>> GetBatchExpense(string expenseid)
    {
        var batchexpense = await _context.BatchExpenses.FindAsync(expenseid);

        if (batchexpense == null)
        {
            return NotFound();
        }

        return batchexpense;
    }

    // PUT: api/BatchExpense/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{expenseid}")]
    public async Task<IActionResult> PutBatchExpense(string? expenseid, BatchExpense batchexpense)
    {
        if (expenseid != batchexpense.ExpenseId)
        {
            return BadRequest();
        }

        _context.Entry(batchexpense).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BatchExpenseExists(expenseid))
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

    // POST: api/BatchExpense
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BatchExpense>> PostBatchExpense(BatchExpense batchexpense)
    {
        _context.BatchExpenses.Add(batchexpense);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBatchExpense", new { expenseid = batchexpense.ExpenseId }, batchexpense);
    }

    // DELETE: api/BatchExpense/5
    [HttpDelete("{expenseid}")]
    public async Task<IActionResult> DeleteBatchExpense(string? expenseid)
    {
        var batchexpense = await _context.BatchExpenses.FindAsync(expenseid);
        if (batchexpense == null)
        {
            return NotFound();
        }

        _context.BatchExpenses.Remove(batchexpense);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BatchExpenseExists(string? expenseid)
    {
        return _context.BatchExpenses.Any(e => e.ExpenseId == expenseid);
    }
}
