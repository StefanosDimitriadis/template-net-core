using FluentValidation;
using Template.Shared.Customers;

namespace Template.Shared.Campaigns
{
	public class AddMoneyRequestValidator : BaseValidator<AddMoneyRequest>
	{
		public AddMoneyRequestValidator()
		{
			RuleFor(_entity => _entity.Money).NotEmpty();
		}
	}
}