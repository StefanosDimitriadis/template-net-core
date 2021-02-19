using FluentValidation;

namespace Template.Shared.Campaigns
{
	public class UpdateCampaignRequestValidator : BaseValidator<UpdateCampaignRequest>
	{
		public UpdateCampaignRequestValidator()
		{
			RuleFor(_entity => _entity.CampaignSpecification).NotEmpty();
			RuleFor(_entity => _entity.CampaignSpecification.BonusMoney).NotEmpty();
		}
	}
}