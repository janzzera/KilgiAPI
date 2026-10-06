using KilgiAPI.Data;
using KilgiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : BaseApiController
    {
        public CustomersController(AppDbContext context) : base(context)
        {
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return await _context.Customers.Where(e => e.UserId == userId).ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{customerid}")]
        public async Task<ActionResult<Customer>> GetCustomer(string customerid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customer = await _context.Customers
                .FirstOrDefaultAsync(e => e.CustomerId == customerid && e.UserId == userId);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        [HttpPut("{customerid}")]
        public async Task<IActionResult> PutCustomer(string? customerid, Customer customer)
        {
            if (customerid != customer.CustomerId)
            {
                return BadRequest();
            }

            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var exists = await _context.Customers
                .AnyAsync(e => e.CustomerId == customerid && e.UserId == userId);
            if (!exists)
            {
                return NotFound();
            }

            customer.UserId = userId;
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customerid, userId))
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

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            customer.UserId = userId;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { customerid = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{customerid}")]
        public async Task<IActionResult> DeleteCustomer(string? customerid)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customer = await _context.Customers
                .FirstOrDefaultAsync(e => e.CustomerId == customerid && e.UserId == userId);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(string? customerid, string userId)
        {
            return _context.Customers.Any(e => e.CustomerId == customerid && e.UserId == userId);
        }
    }
}

