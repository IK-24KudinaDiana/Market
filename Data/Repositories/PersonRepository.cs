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
	public class PersonRepository : Repository<Person>, IPersonRepository
	{
		public PersonRepository(TradeMarketDbContext context) : base(context)
		{
		}
	}
}