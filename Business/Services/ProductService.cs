using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ProductModel>> GetAllAsync()
		{
			var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
			return _mapper.Map<IEnumerable<ProductModel>>(products);
		}

		public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
		{
			var categories = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ProductCategoryModel>>(categories);
		}

		public async Task<ProductModel> GetByIdAsync(int id)
		{
			var product = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
			return _mapper.Map<ProductModel>(product);
		}

		public async Task AddAsync(ProductModel productModel)
		{
			if (string.IsNullOrWhiteSpace(productModel.ProductName))
				throw new MarketException("Product name cannot be empty.");

			if (productModel.Price < 0)
				throw new MarketException("Price cannot be negative.");

			var product = _mapper.Map<Product>(productModel);
			await _unitOfWork.ProductRepository.AddAsync(product);
			await _unitOfWork.SaveAsync();
		}

		public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
		{
			if (string.IsNullOrWhiteSpace(categoryModel.CategoryName))
				throw new MarketException("Category name cannot be empty.");

			var category = _mapper.Map<ProductCategory>(categoryModel);
			await _unitOfWork.ProductCategoryRepository.AddAsync(category);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteAsync(int modelId)
		{
			await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
			await _unitOfWork.SaveAsync();
		}

		public async Task RemoveCategoryAsync(int categoryId)
		{
			await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
			await _unitOfWork.SaveAsync();
		}
		public async Task UpdateAsync(ProductModel productModel)
		{
			if (string.IsNullOrWhiteSpace(productModel.ProductName))
				throw new MarketException("Product name cannot be empty.");

			var product = _mapper.Map<Product>(productModel);
			_unitOfWork.ProductRepository.Update(product);
			await _unitOfWork.SaveAsync();
		}

		public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
		{
			if (string.IsNullOrWhiteSpace(categoryModel.CategoryName))
				throw new MarketException("Category name cannot be empty.");

			var category = _mapper.Map<ProductCategory>(categoryModel);
			_unitOfWork.ProductCategoryRepository.Update(category);
			await _unitOfWork.SaveAsync();
		}

		public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
		{
			var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();

			var filteredProducts = products.AsQueryable();

			if (filterSearch.CategoryId.HasValue)
				filteredProducts = filteredProducts.Where(p => p.ProductCategoryId == filterSearch.CategoryId.Value);

			if (filterSearch.MinPrice.HasValue)
				filteredProducts = filteredProducts.Where(p => p.Price >= filterSearch.MinPrice.Value);

			if (filterSearch.MaxPrice.HasValue)
				filteredProducts = filteredProducts.Where(p => p.Price <= filterSearch.MaxPrice.Value);

			return _mapper.Map<IEnumerable<ProductModel>>(filteredProducts);
		}
	}
}
