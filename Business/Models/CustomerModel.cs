using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
	public class CustomerModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		[CustomValidation(typeof(CustomerModel), nameof(ValidateBirthDate))]
		public DateTime BirthDate { get; set; }

		[Range(0, 100)]
		public int DiscountValue { get; set; }
		public ICollection<int> ReceiptsIds { get; set; }

		public static ValidationResult ValidateBirthDate(DateTime birthDate, ValidationContext context)
		{
			if (birthDate.Year < 1900 || birthDate.Year > 2100)
			{
				return new ValidationResult("Birth date must be between 1900 and 2100.");
			}

			return ValidationResult.Success;
		}
	}
}
