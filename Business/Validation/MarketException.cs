using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validation
{
	public class MarketException : Exception
	{
		public int ErrorCode { get; set; }

		public MarketException() { }

		public MarketException(string message) : base(message) { }

		public MarketException(string message, Exception innerException)
			: base(message, innerException) { }

		public MarketException(string message, int errorCode)
			: base(message)
		{
			ErrorCode = errorCode;
		}

		public override string ToString()
		{
			return $"MarketException: {Message} (Error Code: {ErrorCode})";
		}
	}
}
