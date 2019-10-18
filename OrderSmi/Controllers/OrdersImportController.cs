using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using OrderSmi.Data;
using m = OrderSmi.Model;
namespace OrderSmi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersImportController : ControllerBase
    {
		protected readonly SmiDbContext _context;

		public OrdersImportController(SmiDbContext context)
		{
			_context = context;
		}

		// POST api/values
		[HttpPost]
        public void Post([FromBody] m.OrdersRoot value)
        {
			 _context.Orders.AddRange(value);
			_context.SaveChanges();

		}

    }
}
