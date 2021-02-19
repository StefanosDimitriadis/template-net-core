using Newtonsoft.Json;
using System;
using Template.Domain.Services;
using Template.Domain.Values;

namespace Template.Domain.Entities.Campaigns
{
	public class Campaign : BaseEntity<long>
	{
		public override long Id { get; }
		public CampaignSpecification CampaignSpecification { get; private set; }
		public override DateTime CreatedAt { get; }
		public override DateTime? UpdatedAt { get; protected set; }
		public override bool IsDeleted { get; protected set; }

		private Campaign() { }

		[JsonConstructor]
		private Campaign(long id, CampaignSpecification campaignSpecification, DateTime createdAt, DateTime? updatedAt, bool isDeleted)
		{
			Id = id;
			CampaignSpecification = campaignSpecification;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			IsDeleted = isDeleted;
		}

		private Campaign(long id, CampaignSpecification campaignSpecification, DateTime createdAt)
		{
			Id = id;
			CampaignSpecification = campaignSpecification;
			CreatedAt = createdAt;
		}

		public static Campaign Create(long id, decimal bonusMoney, DateTime createdAt, DateTime? updatedAt, bool isDeleted)
		{
			return new Campaign(id, new CampaignSpecification(bonusMoney), createdAt, updatedAt, isDeleted);
		}

		public static CreateModification<long, long, Campaign> Create(CampaignSpecification campaignSpecification)
		{
			var campaign = new Campaign(IdGenerator.Generate(), campaignSpecification, DateTime.UtcNow);
			var events = new BaseEvent<long, long>[]
			{
				CampaignCreatedEvent.Create(campaign.Id)
			};
			return new CreateModification<long, long, Campaign>(campaign, events);
		}

		public UpdateModification<long, long, Campaign> Update(CampaignSpecification campaignSpecification)
		{
			CampaignSpecification = campaignSpecification;
			UpdatedAt = DateTime.UtcNow;
			var events = new BaseEvent<long, long>[]
			{
				CampaignUpdatedEvent.Create(Id)
			};
			return new UpdateModification<long, long, Campaign>(this, events);
		}

		public DeleteModification<long, long, Campaign> Delete()
		{
			UpdatedAt = DateTime.UtcNow;
			IsDeleted = true;
			var events = new BaseEvent<long, long>[]
			{
				CampaignDeletedEvent.Create(Id)
			};
			return new DeleteModification<long, long, Campaign>(this, events);
		}
	}
}