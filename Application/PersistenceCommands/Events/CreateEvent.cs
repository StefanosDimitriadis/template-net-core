using Template.Domain.Entities;

namespace Template.Application.PersistenceCommands.Events
{
	public class CreateEventPersistenceCommand<TEvent, TId, TEntityId> : BasePersistenceCommand<TId>
		where TEvent : BaseEvent<TId, TEntityId>
	{
		public override TId Id { get; protected set; }
		public TEvent Event { get; }

		public CreateEventPersistenceCommand(TId id, TEvent @event)
		{
			Id = id;
			Event = @event;
		}
	}
}