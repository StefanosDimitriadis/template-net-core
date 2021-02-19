using System;
using Template.Domain.Services;

namespace Template.Domain.Entities.Bonuses
{
	public class BonusCreatedEvent : BaseEvent<long, long>
	{
		private BonusCreatedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static BonusCreatedEvent Create(long entityId)
		{
			return new BonusCreatedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}

	public class BonusDeletedEvent : BaseEvent<long, long>
	{
		private BonusDeletedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static BonusDeletedEvent Create(long entityId)
		{
			return new BonusDeletedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}

	public class BonusAwardedEvent : BaseEvent<long, long>
	{
		private BonusAwardedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static BonusAwardedEvent Create(long entityId)
		{
			return new BonusAwardedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}
}