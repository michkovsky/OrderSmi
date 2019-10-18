using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSmi.Data;
using OrderSmi.Model;

namespace OrderSmi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SmiDbContext _context;

        public OrdersController(SmiDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromBody] string oxidFilter)
        {
			var q = _context.Orders.AsQueryable();

			if (!string.IsNullOrWhiteSpace(oxidFilter))
			{
				q = q.Where(p => p.OxId.Contains(oxidFilter));
			}
            return await q.Include(p=>p.BillingAddress).ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{oxid}")]
        public async Task<ActionResult<Order>> GetOrder(string oxid)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.OxId == oxid);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
		[HttpPut("{oxid}/{invoice}")]
		public async Task<IActionResult> PutInvoiceNumber(string oxid, int invoice)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(p => p.OxId == oxid);
			if (order == null)
			{
				return BadRequest();
			}
			if (order.InvoiceNumber != invoice)
			{
				order.InvoiceNumber = invoice;
				_context.Entry(order).State = EntityState.Modified;
			}

			try
			{
				if (_context.ChangeTracker.HasChanges())
					await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(oxid))
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

		[HttpPut("{oxid}")]
		public async Task<IActionResult> PutStatus(string oxid,[FromBody] string status)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(p => p.OxId == oxid);
			if (order == null)
			{
				return BadRequest();
			}
			var orderStatus = Enum.Parse<OrderStatus>(status,true);
			if (order.Status != orderStatus)
			{
				order.Status = orderStatus;
				_context.Entry(order).State = EntityState.Modified;
			}

			try
			{
				if(_context.ChangeTracker.HasChanges())
					await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(oxid))
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


        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(string oxid)
        {
            return _context.Orders.Any(e => e.OxId == oxid);
        }
    }
}
