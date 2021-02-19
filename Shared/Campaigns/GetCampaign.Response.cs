using System.Collections.Generic;

namespace Template.Shared.Campaigns
{
	public class GetCampaignResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }
		public Campaign Campaign { get; }

		private GetCampaignResponse(Campaign campaign)
		{
			Campaign = campaign;
		}

		private GetCampaignResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static GetCampaignResponse Create(Campaign campaign)
		{
			return new GetCampaignResponse(campaign);
		}

		public static GetCampaignResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new GetCampaignResponse(errors);
		}
	}
}