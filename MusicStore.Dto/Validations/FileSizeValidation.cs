using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Dto.Validations
{
	public class FileSizeValidation : ValidationAttribute
	{
		private readonly int _maxSizeInMegabytes;

		public FileSizeValidation(int maxSizeInMegabytes)
		{
			_maxSizeInMegabytes = maxSizeInMegabytes;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is null)
			{
				return ValidationResult.Success;
			}

			var file = value as IFormFile;
			if (file is null)
			{
				return ValidationResult.Success;
			}

			if (file.Length > _maxSizeInMegabytes * 1024 * 1024)
			{
				return new ValidationResult($"The file size should not exceed {_maxSizeInMegabytes} MB");
			}

			return ValidationResult.Success;
		}
	}
}
