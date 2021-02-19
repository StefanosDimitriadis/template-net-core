using Newtonsoft.Json;
using System;
using Template.Domain.Services;

namespace Template.Domain.Entities.Bonuses
{
	public class Bonus : BaseEntity<long>
	{
		public override long Id { get; }
		public long CampaignId { get; }
		public long CustomerId { get; }
		public bool HasBeenAwarded { get; private set; }
		public override DateTime CreatedAt { get; }
		public override DateTime? UpdatedAt { get; protected set; }
		public override bool IsDeleted { get; protected set; }

		private Bonus() { }

		[JsonConstructor]
		private Bonus(long id, long campaignId, long customerId, bool hasBeenAwarded, DateTime createdAt, DateTime? updatedAt, bool isDeleted)
		{
			Id = id;
			CampaignId = campaignId;
			CustomerId = customerId;
			HasBeenAwarded = hasBeenAwarded;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			IsDeleted = isDeleted;
		}

		private Bonus(long id, long campaignId, long customerId, DateTime createdAt)
		{
			Id = id;
			CampaignId = campaignId;
			CustomerId = customerId;
			CreatedAt = createdAt;
		}

		public static CreateModification<long, long, Bonus> Create(long campaignId, long customerId)
		{
			var bonus = new Bonus(IdGenerator.Generate(), campaignId, customerId, DateTime.UtcNow);
			var events = new BaseEvent<long, long>[]
			{
				BonusCreatedEvent.Create(bonus.Id)
			};
			return new CreateModification<long, long, Bonus>(bonus, events);
		}

		public AwardModification Award()
		{
			HasBeenAwarded = true;
			UpdatedAt = DateTime.UtcNow;
			var events = new BaseEvent<long, long>[]
			{
				BonusAwardedEvent.Create(Id)
			};
			return new AwardModification(this, events);
		}

		public DeleteModification<long, long, Bonus> Delete()
		{
			UpdatedAt = DateTime.UtcNow;
			IsDeleted = true;
			var events = new BaseEvent<long, long>[]
			{
				BonusDeletedEvent.Create(Id)
			};
			return new DeleteModification<long, long, Bonus>(this, events);
		}
	}
}