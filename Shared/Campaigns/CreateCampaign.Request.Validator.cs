using FluentValidation;

namespace Template.Shared.Campaigns
{
	public class CreateCampaignRequestValidator : BaseValidator<CreateCampaignRequest>
	{
		public CreateCampaignRequestValidator()
		{
			RuleFor(_entity => _entity.CampaignSpecification).NotEmpty();
			RuleFor(_entity => _entity.CampaignSpecification.BonusMoney).NotEmpty();
		}
	}
}