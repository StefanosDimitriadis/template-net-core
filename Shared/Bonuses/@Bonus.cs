using System;

namespace Template.Shared.Bonuses
{
	public class Bonus
	{
		public long Id { get; }
		public long CampaignId { get; }
		public long CustomerId { get; }
		public bool HasBeenAwarded { get; }
		public DateTime CreatedAt { get; }
		public DateTime? UpdatedAt { get; }
		public bool IsDeleted { get; }

		public Bonus(long id, long campaignId, long customerId, bool hasBeenAwarded, DateTime createdAt, DateTime? updatedAt, bool isDeleted)
		{
			Id = id;
			CampaignId = campaignId;
			CustomerId = customerId;
			HasBeenAwarded = hasBeenAwarded;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			IsDeleted = isDeleted;
		}
	}
}