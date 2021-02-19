using MongoDB.Driver;
using Template.Domain.Entities;
using Template.Persistence.Settings;

namespace Template.Persistence.DatabaseContexts
{
	public class EventDatabaseContext<TId, TEntityId>
	{
		public IMongoCollection<BaseEvent<TId, TEntityId>> Events { get; set; }

		public EventDatabaseContext(DatabaseContextSettings databaseContextSettings)
		{
			var client = new MongoClient(databaseContextSettings.EventDatabaseSettings.EventDatabaseConnectionString);
			var database = client.GetDatabase(databaseContextSettings.EventDatabaseSettings.EventDatabaseName);
			Events = database.GetCollection<BaseEvent<TId, TEntityId>>(databaseContextSettings.EventDatabaseSettings.EventsCollectionName);
		}
	}
}