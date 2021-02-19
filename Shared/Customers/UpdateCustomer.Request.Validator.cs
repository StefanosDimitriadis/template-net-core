using FluentValidation;
using Template.Shared.Customers;

namespace Template.Shared.Campaigns
{
	public class UpdateCustomerRequestValidator : BaseValidator<UpdateCustomerRequest>
	{
		public UpdateCustomerRequestValidator()
		{
			RuleFor(_entity => _entity.DateOfBirth).NotEmpty();
			RuleFor(_entity => _entity.Email).EmailAddress();
		}
	}
}