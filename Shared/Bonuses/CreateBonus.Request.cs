namespace Template.Shared.Bonuses
{
	public class CreateBonusRequest
	{
		public long CampaignId { get; }
		public long CustomerId { get; }

		public CreateBonusRequest(long campaignId, long customerId)
		{
			CampaignId = campaignId;
			CustomerId = customerId;
		}
	}
}