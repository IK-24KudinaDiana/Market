using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
	public class ReceiptService : IReceiptService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task AddAsync(ReceiptModel model)
		{
			var entity = _mapper.Map<Receipt>(model);
			await _unitOfWork.ReceiptRepository.AddAsync(entity);
			await _unitOfWork.SaveAsync();
		}

		public async Task UpdateAsync(ReceiptModel model)
		{
			var entity = _mapper.Map<Receipt>(model);
			_unitOfWork.ReceiptRepository.Update(entity);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteAsync(int modelId)
		{
			var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
			if (receipt != null)
			{
				foreach (var detail in receipt.ReceiptDetails)
				{
					_unitOfWork.ReceiptDetailRepository.Delete(detail);
				}
				await _unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
				await _unitOfWork.SaveAsync();
			}
		}

		public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
		{
			var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
			return _mapper.Map<IEnumerable<ReceiptModel>>(receipts);
		}

		public async Task<ReceiptModel> GetByIdAsync(int id)
		{
			var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);

			if (receipt == null)
				throw new MarketException("Receipt not found.");

			var receiptModel = _mapper.Map<ReceiptModel>(receipt);
			receiptModel.ReceiptDetailsIds = receipt.ReceiptDetails?.Select(rd => rd.Id).ToList();
			return receiptModel;
		}

		public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
		{
			var receipts = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
			var receiptDetailsModels = _mapper.Map<IEnumerable<ReceiptDetailModel>>(receipts.ReceiptDetails);
			return receiptDetailsModels;
		}

		public async Task<decimal> ToPayAsync(int receiptId)
		{
			var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
			decimal total = 0;
			if (receipt != null)
			{
				foreach (var detail in receipt.ReceiptDetails)
				{
					total += detail.Quantity * detail.DiscountUnitPrice;
				}
			}
			return total;
		}

		public async Task CheckOutAsync(int receiptId)
		{
			var receipt = await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
			if (receipt != null)
			{
				receipt.IsCheckedOut = true;
				_unitOfWork.ReceiptRepository.Update(receipt);
				await _unitOfWork.SaveAsync();
			}
		}
		public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
		{
			var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
			var filteredReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
			return _mapper.Map<IEnumerable<ReceiptModel>>(filteredReceipts);
		}

		public async Task AddProductAsync(int productId, int receiptId, int quantity)
		{
			var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId) ?? throw new MarketException();
			var receiptDetail = receipt.ReceiptDetails?.FirstOrDefault(x => x.ProductId == productId);
			if (receiptDetail == null)
			{
				var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId) ?? throw new MarketException();

				receiptDetail = new ReceiptDetail
				{
					ProductId = productId,
					ReceiptId = receiptId,
					Quantity = quantity,
					UnitPrice = product.Price,
					DiscountUnitPrice = product.Price * (1 - (receipt.Customer.DiscountValue * 0.01m))
				};
				await _unitOfWork.ReceiptDetailRepository.AddAsync(receiptDetail);
				await _unitOfWork.SaveAsync();
				return;
			}
			receiptDetail.Quantity += quantity;
			_unitOfWork.ReceiptRepository.Update(receipt);
			_unitOfWork.ReceiptDetailRepository.Update(receiptDetail);
			await _unitOfWork.SaveAsync();
		}

		public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
		{
			var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
			var receiptDetail = receipt?.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);

			if (receiptDetail == null) return;

			receiptDetail.Quantity -= quantity;

			if (receiptDetail.Quantity <= 0)
			{
				_unitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
			}

			await _unitOfWork.SaveAsync();
		}
	}
}
