using Template.Domain.Entities;

namespace Template.Application.PersistenceCommands.Events
{
	public class UpdateEventPersistenceCommand<TEvent, TId, TEntityId> : BasePersistenceCommand<TId>
		where TEvent : BaseEvent<TId, TEntityId>
	{
		public override TId Id { get; protected set; }
		public TEvent Event { get; }

		public UpdateEventPersistenceCommand(TId id, TEvent @event)
		{
			Id = id;
			Event = @event;
		}
	}
}