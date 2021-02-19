namespace Template.Shared.Campaigns
{
	public class CreateCampaignRequest
	{
		public CampaignSpecification CampaignSpecification { get; }

		public CreateCampaignRequest(CampaignSpecification campaignSpecification)
		{
			CampaignSpecification = campaignSpecification;
		}
	}
}