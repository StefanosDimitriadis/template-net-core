using System.Collections.Generic;

namespace Template.Domain.Entities.Customers
{
	public class AddMoneyModification : IModification<long, long, Customer>
	{
		public Customer Entity { get; }
		public IReadOnlyCollection<BaseEvent<long, long>> Events { get; }

		public AddMoneyModification(Customer entity, IReadOnlyCollection<BaseEvent<long, long>> events)
		{
			Entity = entity;
			Events = events;
		}
	}

	public class AddBonusMoneyModification : AddMoneyModification
	{
		public AddBonusMoneyModification(Customer entity, IReadOnlyCollection<BaseEvent<long, long>> events) : base(entity, events) { }
	}
}