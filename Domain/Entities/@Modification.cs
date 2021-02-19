using System.Collections.Generic;

namespace Template.Domain.Entities
{
	public interface IModification<TEventId, TEntityId, TEntity>
		where TEntity : BaseEntity<TEntityId>
	{
		TEntity Entity { get; }
		IReadOnlyCollection<BaseEvent<TEventId, TEntityId>> Events { get; }
	}

	public class CreateModification<TEventId, TEntityId, TEntity> : IModification<TEventId, TEntityId, TEntity>
		where TEntity : BaseEntity<TEntityId>
	{
		public TEntity Entity { get; }
		public IReadOnlyCollection<BaseEvent<TEventId, TEntityId>> Events { get; }

		public CreateModification(TEntity entity, IReadOnlyCollection<BaseEvent<TEventId, TEntityId>> events)
		{
			Entity = entity;
			Events = events;
		}
	}

	public class DeleteModification<TEventId, TEntityId, TEntity> : IModification<TEventId, TEntityId, TEntity>
		where TEntity : BaseEntity<TEntityId>
	{
		public TEntity Entity { get; }
		public IReadOnlyCollection<BaseEvent<TEventId, TEntityId>> Events { get; }

		public DeleteModification(TEntity entity, IReadOnlyCollection<BaseEvent<TEventId, TEntityId>> events)
		{
			Entity = entity;
			Events = events;
		}
	}

	public class UpdateModification<TEventId, TEntityId, TEntity> : IModification<TEventId, TEntityId, TEntity>
		where TEntity : BaseEntity<TEntityId>
	{
		public TEntity Entity { get; }
		public IReadOnlyCollection<BaseEvent<TEventId, TEntityId>> Events { get; }

		public UpdateModification(TEntity entity, IReadOnlyCollection<BaseEvent<TEventId, TEntityId>> events)
		{
			Entity = entity;
			Events = events;
		}
	}
}