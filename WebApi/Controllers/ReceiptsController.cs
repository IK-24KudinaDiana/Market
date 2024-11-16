using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace WebApi.Controllers
{
	[Route("api/receipts")]
	[ApiController]
	public class ReceiptsController : ControllerBase
	{
		private readonly IReceiptService _receiptService;

		public ReceiptsController(IReceiptService receiptService)
		{
			_receiptService = receiptService;
		}

		// GET api/receipts
		[HttpGet]
		public async Task<IActionResult> GetAllReceipts()
		{
			var receipts = await _receiptService.GetAllAsync();
			return Ok(receipts);
		}

		// GET api/receipts/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetReceiptById(int id)
		{
			try
			{
				var receipt = await _receiptService.GetByIdAsync(id);
				return Ok(receipt);
			}
			catch (MarketException ex)
			{
				return NotFound(ex.Message);
			}
		}

		// GET api/receipts/{id}/details
		[HttpGet("{id}/details")]
		public async Task<IActionResult> GetReceiptDetails(int id)
		{
			try
			{
				var receiptDetails = await _receiptService.GetReceiptDetailsAsync(id);
				return Ok(receiptDetails);
			}
			catch (MarketException ex)
			{
				return NotFound(ex.Message);
			}
		}

		// GET api/receipts/{id}/sum
		[HttpGet("{id}/sum")]
		public async Task<IActionResult> GetSum(int id)
		{
			try
			{
				var sum = await _receiptService.ToPayAsync(id);
				return Ok(sum);
			}
			catch (MarketException)
			{
				return NotFound();
			}
		}

		// GET api/receipts/period?startDate=2021-12-1&endDate=2021-12-31
		[HttpGet("period")]
		public async Task<IActionResult> GetReceiptsByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
		{
			var receipts = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
			return Ok(receipts);
		}

		// POST api/receipts
		[HttpPost]
		public async Task<IActionResult> AddReceipt([FromBody] ReceiptModel model)
		{
			try
			{
				await _receiptService.AddAsync(model);
				return CreatedAtAction(nameof(GetReceiptById), new { id = model.Id }, model);
			}
			catch (MarketException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT api/receipts/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateReceipt(int id, [FromBody] ReceiptModel model)
		{
			if (id != model.Id)
			{
				return BadRequest("Receipt ID mismatch.");
			}

			try
			{
				await _receiptService.UpdateAsync(model);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return NotFound(ex.Message);
			}
		}

		// PUT api/receipts/{id}/products/add/{productId}/{quantity}
		[HttpPut("{id}/products/add/{productId}/{quantity}")]
		public async Task<IActionResult> AddProductToReceipt(int id, int productId, int quantity)
		{
			try
			{
				await _receiptService.AddProductAsync(productId, id, quantity);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT api/receipts/{id}/products/remove/{productId}/{quantity}
		[HttpPut("{id}/products/remove/{productId}/{quantity}")]
		public async Task<IActionResult> RemoveProductFromReceipt(int id, int productId, int quantity)
		{
			try
			{
				await _receiptService.RemoveProductAsync(productId, id, quantity);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT api/receipts/{id}/checkout
		[HttpPut("{id}/checkout")]
		public async Task<IActionResult> CheckOutReceipt(int id)
		{
			try
			{
				await _receiptService.CheckOutAsync(id);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return NotFound(ex.Message);
			}
		}

		// DELETE api/receipts/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteReceipt(int id)
		{
			try
			{
				await _receiptService.DeleteAsync(id);
				return NoContent();
			}
			catch (MarketException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}