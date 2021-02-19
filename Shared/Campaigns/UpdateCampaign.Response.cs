using System.Collections.Generic;

namespace Template.Shared.Campaigns
{
	public class UpdateCampaignResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private UpdateCampaignResponse() { }

		private UpdateCampaignResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static UpdateCampaignResponse Create()
		{
			return new UpdateCampaignResponse();
		}

		public static UpdateCampaignResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new UpdateCampaignResponse(errors);
		}
	}
}