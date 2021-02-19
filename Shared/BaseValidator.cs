using FluentValidation;
using FluentValidation.Results;
using System;
using System.Net.Mail;

namespace Template.Shared
{
	public abstract class BaseValidator<TEntity> : AbstractValidator<TEntity>
	{
		public override ValidationResult Validate(ValidationContext<TEntity> validationContext)
		{
			var validationResult = base.Validate(validationContext);

			if (!validationResult.IsValid)
				throw new ValidationException(validationResult.Errors);

			return validationResult;
		}
	}

	public static class CustomValidations
	{
		public static IRuleBuilderOptions<T, string> EmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			return ruleBuilder.Must(_property =>
				{
					return ValidateInput(ref _property);
				})
				.WithMessage("{PropertyName} must be in valid format");
		}

		private static bool ValidateInput(ref string _property)
		{
			try
			{
				var mailAddress = new MailAddress(_property);
				return mailAddress.Address == _property;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}