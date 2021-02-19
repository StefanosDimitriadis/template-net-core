namespace Template.Shared.Campaigns
{
	public class DeleteCampaignRequest
	{
		public long Id { get; }

		public DeleteCampaignRequest(long id)
		{
			Id = id;
		}
	}
}