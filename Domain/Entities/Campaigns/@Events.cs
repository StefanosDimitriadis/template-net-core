using System;
using Template.Domain.Services;

namespace Template.Domain.Entities.Campaigns
{
	public class CampaignCreatedEvent : BaseEvent<long, long>
	{
		private CampaignCreatedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static CampaignCreatedEvent Create(long entityId)
		{
			return new CampaignCreatedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}

	public class CampaignUpdatedEvent : BaseEvent<long, long>
	{
		private CampaignUpdatedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static CampaignUpdatedEvent Create(long entityId)
		{
			return new CampaignUpdatedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}

	public class CampaignDeletedEvent : BaseEvent<long, long>
	{
		private CampaignDeletedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static CampaignDeletedEvent Create(long entityId)
		{
			return new CampaignDeletedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}
}