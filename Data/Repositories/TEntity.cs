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
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
	{
		protected readonly TradeMarketDbContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		public Repository(TradeMarketDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<TEntity>();
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public virtual async Task<TEntity> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public virtual async Task AddAsync(TEntity entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public void Delete(TEntity entity)
		{
			_dbSet.Remove(entity);
		}

		public async Task DeleteByIdAsync(int id)
		{
			var entity = await GetByIdAsync(id);
			if (entity != null)
			{
				Delete(entity);
			}
		}

		public virtual void Update(TEntity entity)
		{
			_dbSet.Update(entity);
		}
	}
}
