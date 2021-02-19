using Newtonsoft.Json;

namespace Template.Shared.Campaigns
{
	public class UpdateCampaignRequest
	{
		[JsonIgnore]
		public long Id { get; private set; }
		public CampaignSpecification CampaignSpecification { get; }

		public UpdateCampaignRequest(CampaignSpecification campaignSpecification)
		{
			CampaignSpecification = campaignSpecification;
		}

		public void SetId(long id)
		{
			Id = id;
		}
	}
}