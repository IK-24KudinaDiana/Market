﻿using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
	public class ProductModel 
	{
		public int Id { get; set; }
		public int ProductCategoryId { get; set; }
		public string CategoryName { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public ICollection<int> ReceiptDetailIds { get; set; }
	}
}
