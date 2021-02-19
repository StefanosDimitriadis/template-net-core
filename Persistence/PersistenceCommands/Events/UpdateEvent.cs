using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Application.PersistenceCommands.Events;
using Template.Domain.Entities;

namespace Template.Persistence.PersistenceCommands.Events
{
	internal class UpdateEventCommandPersistence : ICommandPersistence<long, UpdateEventPersistenceCommand<BaseEvent<long, long>, long, long>>
	{
		private readonly IEventCommandStorage<long, long> _eventCommandStorage;

		public UpdateEventCommandPersistence(IEventCommandStorage<long, long> eventCommandStorage)
		{
			_eventCommandStorage = eventCommandStorage;
		}

		public async Task Persist(UpdateEventPersistenceCommand<BaseEvent<long, long>, long, long> persistCommand)
		{
			await _eventCommandStorage.Update(persistCommand.Event);
		}
	}
}