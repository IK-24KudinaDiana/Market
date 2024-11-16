using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly TradeMarketDbContext _context;

		private ICustomerRepository _customerRepository;
		private IPersonRepository _personRepository;
		private IProductRepository _productRepository;
		private IProductCategoryRepository _productCategoryRepository;
		private IReceiptRepository _receiptRepository;
		private IReceiptDetailRepository _receiptDetailRepository;

		public UnitOfWork(TradeMarketDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public ICustomerRepository CustomerRepository
			=> _customerRepository ??= new CustomerRepository((TradeMarketDbContext)_context);

		public IPersonRepository PersonRepository
			=> _personRepository ??= new PersonRepository((TradeMarketDbContext)_context);

		public IProductRepository ProductRepository
			=> _productRepository ??= new ProductRepository((TradeMarketDbContext)_context);

		public IProductCategoryRepository ProductCategoryRepository
			=> _productCategoryRepository ??= new ProductCategoryRepository((TradeMarketDbContext)_context);

		public IReceiptRepository ReceiptRepository
			=> _receiptRepository ??= new ReceiptRepository((TradeMarketDbContext)_context);

		public IReceiptDetailRepository ReceiptDetailRepository
			=> _receiptDetailRepository ??= new ReceiptDetailRepository((TradeMarketDbContext)_context);
		
		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}