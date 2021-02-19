using FluentValidation;
using Template.Shared.Bonuses;

namespace Template.Shared.Campaigns
{
	public class AwardBonusRequestValidator : BaseValidator<AwardBonusRequest>
	{
		public AwardBonusRequestValidator()
		{
			RuleFor(_entity => _entity.Id).NotEmpty();
		}
	}
}