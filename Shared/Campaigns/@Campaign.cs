using System;

namespace Template.Shared.Campaigns
{
	public class Campaign
	{
		public long Id { get; }
		public CampaignSpecification CampaignSpecification { get; }
		public DateTime CreatedAt { get; }
		public DateTime? UpdatedAt { get; }
		public bool IsDeleted { get; }

		public Campaign(long id, CampaignSpecification campaignSpecification, DateTime createdAt, DateTime? updatedAt, bool isDeleted)
		{
			Id = id;
			CampaignSpecification = campaignSpecification;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			IsDeleted = isDeleted;
		}
	}
}