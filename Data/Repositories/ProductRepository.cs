using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(TradeMarketDbContext context) : base(context)
		{
		}

		public override async Task AddAsync(Product entity)
		{
			entity.Category = await _context.ProductCategories.FindAsync(entity.ProductCategoryId);
			await base.AddAsync(entity);
		}

		public override void Update(Product entity)
		{
			var product = _context.Products.Find(entity.Id);
			product.ProductName = entity.ProductName;
			product.Price = entity.Price;
			product.ProductCategoryId = entity.ProductCategoryId;
			base.Update(product);
		}
		public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
		{
			return await _dbSet.Include(p => p.Category)
							   .Include(p => p.ReceiptDetails)
							   .ToListAsync();
		}

		public async Task<Product> GetByIdWithDetailsAsync(int id)
		{
			return await _dbSet.Include(p => p.Category)
							   .Include(p => p.ReceiptDetails)
							   .FirstOrDefaultAsync(p => p.Id == id);
		}
	}
}