using System.Collections.Generic;

namespace Template.Shared.Campaigns
{
	public class DeleteCampaignResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private DeleteCampaignResponse() { }

		private DeleteCampaignResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static DeleteCampaignResponse Create()
		{
			return new DeleteCampaignResponse();
		}

		public static DeleteCampaignResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new DeleteCampaignResponse(errors);
		}
	}
}