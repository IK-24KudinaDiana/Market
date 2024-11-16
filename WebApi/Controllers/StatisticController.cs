using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace WebApi.Controllers
{
	[Route("api/statistics")]
	[ApiController]
	public class StatisticController : ControllerBase
	{
		private readonly IStatisticService _statisticService;

		public StatisticController(IStatisticService statisticService)
		{
			_statisticService = statisticService;
		}

		// GET api/statistics/popularProducts?productCount=1
		[HttpGet("popularProducts")]
		public async Task<IActionResult> GetMostPopularProducts([FromQuery] int productCount)
		{
			var products = await _statisticService.GetMostPopularProductsAsync(productCount);
			return Ok(products);
		}

		// GET api/statistics/customer/{id}/{productCount}
		[HttpGet("customer/{id}/{productCount}")]
		public async Task<IActionResult> GetCustomerMostPopularProducts(int id, int productCount)
		{
			var products = await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id);
			return Ok(products);
		}

		// GET api/statistics/activity/{customerCount}?startDate=2020-7-21&endDate=2022-7-22
		[HttpGet("activity/{customerCount}")]
		public async Task<IActionResult> GetMostValuableCustomers(int customerCount, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
		{
			var customers = await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate);
			return Ok(customers);
		}

		// GET api/statistics/income/{categoryId}?startDate=2021-7-25&endDate=2021-10-20
		[HttpGet("income/{categoryId}")]
		public async Task<IActionResult> GetIncomeOfCategory([FromRoute] int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
		{
			var income = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
			return Ok(income);
		}
	}
}