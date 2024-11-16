using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
	public class StatisticService : IStatisticService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
		{
			var receiptDetails = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();
			var productSales = receiptDetails
				.GroupBy(r => r.ProductId)
				.Select(g => new
				{
					Product = g.FirstOrDefault()?.Product,
					SalesCount = g.Sum(r => r.Quantity),
				})
				.OrderByDescending(x => x.SalesCount)
				.Take(productCount)
				.Select(x => _mapper.Map<ProductModel>(x.Product));

			return productSales;
		}

		public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
		{
			var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
			var customerReceipts = receipts.Where(r => r.CustomerId == customerId);

			var receiptDetails = customerReceipts
				.SelectMany(r => r.ReceiptDetails)
				.GroupBy(r => r.ProductId)
				.Select(g => new
				{
					Product = g.FirstOrDefault()?.Product,
					SalesCount = g.Sum(r => r.Quantity),
				})
				.OrderByDescending(x => x.SalesCount)
				.Take(productCount)
				.Select(x => _mapper.Map<ProductModel>(x.Product));

			return receiptDetails;
		}

		public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
		{
			var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

			var filteredReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate).ToList();

			var customerGroups = filteredReceipts
				.GroupBy(r => r.Customer)
				.Select(group => new
				{
					Customer = group.Key,
					TotalSum = group.Sum(r => r.ReceiptDetails.Sum(rd => rd.DiscountUnitPrice * rd.Quantity)),
				})
				.OrderByDescending(c => c.TotalSum)
				.Take(customerCount) 
				.ToList();

			var customerActivityModels = customerGroups.Select(c => new CustomerActivityModel
			{
				CustomerId = c.Customer.Id,
				CustomerName = c.Customer.Person.Name + " " + c.Customer.Person.Surname,
				ReceiptSum = c.TotalSum
			});

			return customerActivityModels;
		}

		public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
		{
			var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

			var filteredReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate).ToList();

			var productDetailsInCategory = filteredReceipts
				.SelectMany(r => r.ReceiptDetails)
				.Where(rd => rd.Product.ProductCategoryId == categoryId)
				.ToList();

			decimal totalIncome = productDetailsInCategory.Sum(rd => rd.DiscountUnitPrice * rd.Quantity);

			return totalIncome;
		}
	}
}
