using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options)
        {
        }
		public DbSet<Person> Persons { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Receipt> Receipts { get; set; }
		public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Customer>()
				.HasOne(c => c.Person)
				.WithOne()
				.HasForeignKey<Customer>(c => c.PersonId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Receipt>()
				.HasOne(r => r.Customer)
				.WithMany(c => c.Receipts)
				.HasForeignKey(r => r.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<ReceiptDetail>().HasKey(k => new { k.ReceiptId, k.ProductId });

			modelBuilder.Entity<ReceiptDetail>()
				.HasOne(rd => rd.Receipt)
				.WithMany(r => r.ReceiptDetails)
				.HasForeignKey(rd => rd.ReceiptId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ReceiptDetail>()
				.HasOne(rd => rd.Product)
				.WithMany(p => p.ReceiptDetails)
				.HasForeignKey(rd => rd.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Category)
				.WithMany(pc => pc.Products)
				.HasForeignKey(p => p.ProductCategoryId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
