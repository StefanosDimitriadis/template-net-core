using FluentValidation;
using Template.Shared.Bonuses;

namespace Template.Shared.Campaigns
{
	public class DeleteBonusRequestValidator : BaseValidator<DeleteBonusRequest>
	{
		public DeleteBonusRequestValidator()
		{
			RuleFor(_entity => _entity.Id).NotEmpty();
		}
	}
}