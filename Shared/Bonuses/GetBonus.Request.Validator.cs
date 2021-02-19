using FluentValidation;
using Template.Shared.Bonuses;

namespace Template.Shared.Campaigns
{
	public class GetBonusRequestValidator : BaseValidator<GetBonusRequest>
	{
		public GetBonusRequestValidator()
		{
			RuleFor(_entity => _entity.Id).NotEmpty();
		}
	}
}