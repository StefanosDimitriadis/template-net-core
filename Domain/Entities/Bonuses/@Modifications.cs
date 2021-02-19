using System.Collections.Generic;

namespace Template.Domain.Entities.Bonuses
{
	public class AwardModification : IModification<long, long, Bonus>
	{
		public Bonus Entity { get; }
		public IReadOnlyCollection<BaseEvent<long, long>> Events { get; }

		public AwardModification(Bonus entity, IReadOnlyCollection<BaseEvent<long, long>> events)
		{
			Entity = entity;
			Events = events;
		}
	}
}