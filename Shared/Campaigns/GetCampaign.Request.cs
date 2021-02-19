namespace Template.Shared.Campaigns
{
	public class GetCampaignRequest
	{
		public long Id { get; }

		public GetCampaignRequest(long id)
		{
			Id = id;
		}
	}
}