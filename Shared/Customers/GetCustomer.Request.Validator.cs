using FluentValidation;
using Template.Shared.Customers;

namespace Template.Shared.Campaigns
{
	public class GetCustomerRequestValidator : BaseValidator<GetCustomerRequest>
	{
		public GetCustomerRequestValidator()
		{
			RuleFor(_entity => _entity.Id).NotEmpty();
		}
	}
}