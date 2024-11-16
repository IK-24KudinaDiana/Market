using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly ICustomerService _customerService;

		public CustomersController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

		// GET: api/customers
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerModel>>> Get()
		{
			var customers = await _customerService.GetAllAsync();
			return Ok(customers);
		}

		//GET: api/customers/1
		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerModel>> GetById(int id)
		{
			try
			{
				var customer = await _customerService.GetByIdAsync(id);
				return Ok(customer);
			}
			catch (MarketException ex)
			{
				return NotFound(ex.Message);
			}
		}

		//GET: api/customers/products/1
		[HttpGet("products/{id}")]
		public async Task<ActionResult<CustomerModel>> GetByProductId(int id)
		{
			var customers = await _customerService.GetCustomersByProductIdAsync(id);
			if (!customers.Any())
			{
				return NotFound($"No customers found for product with ID {id}.");
			}
			return Ok(customers);
		}

		// POST: api/customers
		[HttpPost]
		public async Task<ActionResult> Post([FromBody] CustomerModel value)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _customerService.AddAsync(value);
				return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
			}
			catch (MarketException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: api/customers/1
		[HttpPut("{id}")]
		public async Task<ActionResult> Put(int Id, [FromBody] CustomerModel value)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _customerService.UpdateAsync(value);
			return Ok(value);
		}

		// DELETE: api/customers/1
		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			await _customerService.DeleteAsync(id);
			return NoContent();
		}
	}
}