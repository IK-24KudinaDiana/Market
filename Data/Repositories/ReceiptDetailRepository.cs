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
	public class ReceiptDetailRepository : Repository<ReceiptDetail>, IReceiptDetailRepository
	{
		public ReceiptDetailRepository(TradeMarketDbContext context) : base(context)
		{
		}

		public override async Task<ReceiptDetail> GetByIdAsync(int id)
		{
			return await _context.ReceiptsDetails.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
		{
			return await _dbSet.Include(rd => rd.Receipt)
							   .Include(rd => rd.Product)
									.ThenInclude(p => p.Category)
							   .ToListAsync();
		}
	}
}