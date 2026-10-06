using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class RetailSalesController : BaseApiController
    {
        public RetailSalesController(AppDbContext context) : base(context)
        {
        }

        // GET: api/RetailSales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetailSale>>> GetRetailSale()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.RetailSales.Where(e => e.UserId == userId).ToListAsync();
        }

        // GET: api/RetailSales/5
        [HttpGet("{saleid}")]
        public async Task<ActionResult<RetailSale>> GetRetailSale(string saleid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var retailsale = await _context.RetailSales
                .FirstOrDefaultAsync(e => e.SaleId == saleid && e.UserId == userId);

            if (retailsale == null)
            {
                return NotFound();
            }

            return retailsale;
        }

        // PUT: api/RetailSales/5
        [HttpPut("{saleid}")]
        public async Task<IActionResult> PutRetailSale(string? saleid, RetailSale retailsale)
        {
            if (saleid != retailsale.SaleId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.RetailSales
                .AnyAsync(e => e.SaleId == saleid && e.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            retailsale.UserId = userId;
            _context.Entry(retailsale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RetailSaleExists(saleid, userId))
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

        // POST: api/RetailSales
        [HttpPost]
        public async Task<ActionResult<RetailSale>> PostRetailSale(RetailSale retailsale)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            retailsale.UserId = userId;
            _context.RetailSales.Add(retailsale);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRetailSale", new { saleid = retailsale.SaleId }, retailsale);
        }

        // DELETE: api/RetailSales/5
        [HttpDelete("{saleid}")]
        public async Task<IActionResult> DeleteRetailSale(string? saleid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var retailsale = await _context.RetailSales
                .FirstOrDefaultAsync(e => e.SaleId == saleid && e.UserId == userId);
            if (retailsale == null)
            {
                return NotFound();
            }

            _context.RetailSales.Remove(retailsale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RetailSaleExists(string? saleid, string userId)
        {
            return _context.RetailSales.Any(e => e.SaleId == saleid && e.UserId == userId);
        }
    }
}

