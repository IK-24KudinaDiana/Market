using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<CustomerModel>> GetAllAsync()
		{
			var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
			if (customers == null)
				return Enumerable.Empty<CustomerModel>();
			return _mapper.Map<IEnumerable<CustomerModel>>(customers);
		}

		public async Task<CustomerModel> GetByIdAsync(int id)
		{
			var customer = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
			if (customer == null)
				throw new MarketException($"Customer with id {id} not found.");

			return _mapper.Map<CustomerModel>(customer);
		}

		public async Task AddAsync(CustomerModel customerModel)
		{
			if (customerModel == null)
				throw new MarketException("Customer model cannot be null.");


			if (string.IsNullOrWhiteSpace(customerModel.Name))
				throw new MarketException("Customer name cannot be empty.");

			if (customerModel.BirthDate < new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc) || customerModel.BirthDate > DateTime.Now)
				throw new MarketException("Invalid birth date.");

			var customer = _mapper.Map<Customer>(customerModel);
			await _unitOfWork.CustomerRepository.AddAsync(customer);
			await _unitOfWork.SaveAsync();
		}

		public async Task UpdateAsync(CustomerModel customerModel)
		{
			if (customerModel == null)
				throw new MarketException("Customer model cannot be null.");

			if (string.IsNullOrWhiteSpace(customerModel.Surname))
				throw new MarketException("Customer surname cannot be empty.");
			if (string.IsNullOrWhiteSpace(customerModel.Name))
				throw new MarketException("Customer name cannot be empty.");

			if (customerModel.BirthDate < new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc) || customerModel.BirthDate > DateTime.Now)
				throw new MarketException("Invalid birth date.");

			var customer = _mapper.Map<Customer>(customerModel);
			var person = _mapper.Map<Person>(customerModel);
			_unitOfWork.PersonRepository.Update(person);
			_unitOfWork.CustomerRepository.Update(customer);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteAsync(int modelId)
		{
			await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
			await _unitOfWork.SaveAsync();
		}

		public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
		{
			var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();

			var filteredCustomers = customers
				.Where(c => c.Receipts
					.Any(r => r.ReceiptDetails
						.Any(rd => rd.ProductId == productId)))
				.ToList();

			if (!filteredCustomers.Any())
			{
				return Enumerable.Empty<CustomerModel>();
			}

			return _mapper.Map<IEnumerable<CustomerModel>>(filteredCustomers);
		}
	}
}
