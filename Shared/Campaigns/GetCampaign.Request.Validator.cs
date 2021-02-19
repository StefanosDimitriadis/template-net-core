using FluentValidation;

namespace Template.Shared.Campaigns
{
	public class GetCampaignRequestValidator : BaseValidator<GetCampaignRequest>
	{
		public GetCampaignRequestValidator()
		{
			RuleFor(_entity => _entity.Id).NotEmpty();
		}
	}
}