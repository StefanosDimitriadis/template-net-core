using MongoDB.Driver;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities;
using Template.Persistence.DatabaseContexts;

namespace Template.Persistence.Storages
{
	internal class EventCommandStorage<TId, TEventId> : IEventCommandStorage<TId, TEventId>
	{
		private readonly EventDatabaseContext<TId, TEventId> _eventDatabaseContext;

		public EventCommandStorage(EventDatabaseContext<TId, TEventId> eventDatabaseContext)
		{
			_eventDatabaseContext = eventDatabaseContext;
		}

		public async Task Create(BaseEvent<TId, TEventId> @event)
		{
			await _eventDatabaseContext.Events.InsertOneAsync(@event);
		}

		public async Task Update(BaseEvent<TId, TEventId> @event)
		{
			await _eventDatabaseContext.Events.ReplaceOneAsync(_event => _event.Id.Equals(@event.Id), @event);
		}
	}
}