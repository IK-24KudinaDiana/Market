﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
	public class ReceiptDetail : BaseEntity
	{
		public int ReceiptId { get; set; }

		public int ProductId { get; set; }

		public decimal DiscountUnitPrice { get; set; }

		public decimal UnitPrice { get; set; }

		public int Quantity { get; set; }

		public Receipt Receipt { get; set; }

		public Product Product { get; set; }
	}
}
