using System;
using Template.Domain.Services;

namespace Template.Domain.Entities.Customers
{
	public class CustomerCreatedEvent : BaseEvent<long, long>
	{
		private CustomerCreatedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static CustomerCreatedEvent Create(long entityId)
		{
			return new CustomerCreatedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}

	public class CustomerUpdatedEvent : BaseEvent<long, long>
	{
		private CustomerUpdatedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static CustomerUpdatedEvent Create(long entityId)
		{
			return new CustomerUpdatedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}

	public class CustomerDeletedEvent : BaseEvent<long, long>
	{
		private CustomerDeletedEvent(long id, long entityId, DateTime createdAt)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
		}

		public static CustomerDeletedEvent Create(long entityId)
		{
			return new CustomerDeletedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}

	public class CustomerMoneyAddedEvent : BaseEvent<long, long>
	{
		public decimal InitialMoney { get; }
		public decimal MoneyToAdd { get; }
		public decimal FinalMoney { get; }

		private CustomerMoneyAddedEvent(long id, long entityId, DateTime createdAt, decimal initialMoney, decimal moneyToAdd, decimal finalMoney)
		{
			Id = id;
			EntityId = entityId;
			CreatedAt = createdAt;
			InitialMoney = initialMoney;
			MoneyToAdd = moneyToAdd;
			FinalMoney = finalMoney;
		}

		public static CustomerMoneyAddedEvent Create(long entityId, decimal initialMoney, decimal moneyToAdd, decimal finalMoney)
		{
			return new CustomerMoneyAddedEvent(IdGenerator.Generate(), entityId, DateTime.UtcNow, initialMoney, moneyToAdd, finalMoney);
		}

		public override void SetHandled()
		{
			HandledAt = DateTime.UtcNow;
			IsHandled = true;
		}
	}
}