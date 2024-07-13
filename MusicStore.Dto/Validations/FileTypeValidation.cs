using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Dto.Validations
{
	public class FileTypeValidation : ValidationAttribute
	{
		private readonly string[] _validTypes;

		public FileTypeValidation(string[] validTypes)
		{
			_validTypes = validTypes;
		}

		public FileTypeValidation(FileTypeGroup fileTypeGroup)
		{
			if (fileTypeGroup is FileTypeGroup.Image)
			{
				_validTypes = ["image/jpeg", "image/png", "image/jpg"];
			}
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is null)
			{
				return ValidationResult.Success;
			}

			IFormFile? formFile = value as IFormFile;
			if (formFile is null)
			{
				return ValidationResult.Success;
			}

			if (!_validTypes.Contains(formFile.ContentType))
			{
				return new ValidationResult($"The file type should be one of the following: {string.Join(", ", _validTypes)}");
			}

			return ValidationResult.Success;
		}
	}

	public enum FileTypeGroup
	{
		Image,
		Audio,
		Video
	}
}
