using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
	public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
	{
		public ProductCategoryRepository(TradeMarketDbContext context) : base(context)
		{
		}
	}
}