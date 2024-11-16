using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		// GET: api/products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductModel>>> Get([FromQuery] FilterSearchModel? filterModel)
		{
			IEnumerable<ProductModel> products;
			if (filterModel is null)
				products = await _productService.GetAllAsync();
			else
				products = await _productService.GetByFilterAsync(filterModel);
			return Ok(products);
		}

		// GET: api/products/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductModel>> GetProductById(int id)
		{
			var product = await _productService.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		// POST: api/products
		[HttpPost]
		public async Task<ActionResult<ProductModel>> AddProduct([FromBody] ProductModel product)
		{
			if (product == null || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _productService.AddAsync(product);
				return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
			}
			catch (MarketException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: api/products/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductModel product)
		{
			if (id != product.Id || !ModelState.IsValid)
			{
				return BadRequest();
			}

			try
			{
				await _productService.UpdateAsync(product);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: api/products/{id}
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteProduct(int id)
		{
			try
			{
				await _productService.DeleteAsync(id);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return NotFound(ex.Message);
			}
		}

		// GET: api/products/categories
		[HttpGet("categories")]
		public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetAllCategories()
		{
			var categories = await _productService.GetAllProductCategoriesAsync();
			return Ok(categories);
		}

		// POST: api/products/categories
		[HttpPost("categories")]
		public async Task<ActionResult<ProductCategoryModel>> AddCategory([FromBody] ProductCategoryModel category)
		{
			if (category == null || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _productService.AddCategoryAsync(category);
				return CreatedAtAction(nameof(GetAllCategories), category);
			}
			catch (MarketException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: api/products/categories/{id}
		[HttpPut("categories/{id}")]
		public async Task<ActionResult> UpdateCategory(int id, [FromBody] ProductCategoryModel category)
		{
			if (id != category.Id || !ModelState.IsValid)
			{
				return BadRequest();
			}

			try
			{
				await _productService.UpdateCategoryAsync(category);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: api/products/categories/{id}
		[HttpDelete("categories/{id}")]
		public async Task<ActionResult> DeleteCategory(int id)
		{
			try
			{
				await _productService.RemoveCategoryAsync(id);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}