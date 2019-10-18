using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using m = OrderSmi.Model;
namespace OrderSmi.Data
{
	public class SmiDbContext:DbContext
	{
		public SmiDbContext(DbContextOptions<SmiDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			{
				var ent = modelBuilder.Entity<m.Order>();
				ent.HasAlternateKey(p => p.OxId);
				ent.Property(p => p.OxId).HasMaxLength(16);
			}
			{
				var ent = modelBuilder.Entity<m.Address>();
				ent.Ignore(p => p.CountryInternal);
			}
			{
				var ent = modelBuilder.Entity<m.Article>();
				ent.Property(p => p.ArticleNumber).HasMaxLength(13);
			}
		}

		public DbSet<m.Order> Orders { get; set; }

	}
}
