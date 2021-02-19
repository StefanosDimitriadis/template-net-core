using FluentValidation;
using Template.Shared.Bonuses;

namespace Template.Shared.Campaigns
{
	public class CreateBonusRequestValidator : BaseValidator<CreateBonusRequest>
	{
		public CreateBonusRequestValidator()
		{
			RuleFor(_entity => _entity.CampaignId).NotEmpty();
			RuleFor(_entity => _entity.CustomerId).NotEmpty();
		}
	}
}