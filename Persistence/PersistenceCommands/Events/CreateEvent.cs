using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Application.PersistenceCommands.Events;
using Template.Domain.Entities;

namespace Template.Persistence.PersistenceCommands.Events
{
	internal class CreateEventCommandPersistence : ICommandPersistence<long, CreateEventPersistenceCommand<BaseEvent<long, long>, long, long>>
	{
		private readonly IEventCommandStorage<long, long> _eventCommandStorage;

		public CreateEventCommandPersistence(IEventCommandStorage<long, long> eventCommandStorage)
		{
			_eventCommandStorage = eventCommandStorage;
		}

		public async Task Persist(CreateEventPersistenceCommand<BaseEvent<long, long>, long, long> persistCommand)
		{
			await _eventCommandStorage.Create(persistCommand.Event);
		}
	}
}