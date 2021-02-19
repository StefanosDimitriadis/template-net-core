using System.Collections.Generic;

namespace Template.Shared.Campaigns
{
	public class CreateCampaignResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private CreateCampaignResponse() { }

		private CreateCampaignResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static CreateCampaignResponse Create()
		{
			return new CreateCampaignResponse();
		}

		public static CreateCampaignResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new CreateCampaignResponse(errors);
		}
	}
}