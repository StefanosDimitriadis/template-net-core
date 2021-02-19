using FluentValidation;
using Template.Shared.Customers;

namespace Template.Shared.Campaigns
{
	public class DeleteCustomerRequestValidator : BaseValidator<DeleteCustomerRequest>
	{
		public DeleteCustomerRequestValidator()
		{
			RuleFor(_entity => _entity.Id).NotEmpty();
		}
	}
}