using FluentValidation;
using Template.Shared.Customers;

namespace Template.Shared.Campaigns
{
	public class CreateCustomerRequestValidator : BaseValidator<CreateCustomerRequest>
	{
		public CreateCustomerRequestValidator()
		{
			RuleFor(_entity => _entity.Name).NotEmpty();
			RuleFor(_entity => _entity.Email).EmailAddress();
			RuleFor(_entity => _entity.DateOfBirth).NotEmpty();
		}
	}
}