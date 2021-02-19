using FluentValidation;

namespace Template.Shared.Campaigns
{
	public class DeleteCampaignRequestValidator : BaseValidator<DeleteCampaignRequest>
	{
		public DeleteCampaignRequestValidator()
		{
			RuleFor(_entity => _entity.Id).NotEmpty();
		}
	}
}