using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Template.Application.Services;

namespace Template.Persistence.Settings
{
	public class DatabaseContextSettings : IValidatable
	{
		public const string SettingsName = nameof(DatabaseContextSettings);

		public string BonusDatabaseConnectionString { get; set; }
		public string CampaignDatabaseConnectionString { get; set; }
		public string CustomerDatabaseConnectionString { get; set; }
		public EventDatabaseSettings EventDatabaseSettings { get; set; }

		public void Validate()
		{
			var connectionStrings = new Dictionary<string, string>
			{
				{ "Bonus", BonusDatabaseConnectionString },
				{ "Campaign", CampaignDatabaseConnectionString },
				{ "Customer", CustomerDatabaseConnectionString }
			};

			foreach (var connectionString in connectionStrings)
			{
				if (string.IsNullOrWhiteSpace(connectionString.Value))
					throw new ArgumentException(message: $"Connection string for {connectionString.Key} database cannot be null", paramName: nameof(connectionString.Value));

				try
				{
					var dbConnectionStringBuilder = new DbConnectionStringBuilder
					{
						ConnectionString = connectionString.Value
					};
				}
				catch (Exception exception)
				{
					throw new ArgumentException(message: $"Use valid connection string for {connectionString.Key} database", paramName: nameof(connectionString.Value), exception);
				}
			}

			try
			{
				var client = new MongoClient(EventDatabaseSettings.EventDatabaseConnectionString);
				var databases = client.ListDatabaseNames();
				if (databases.ToList().Count < 1)
					throw new Exception();
			}
			catch (Exception exception)
			{
				throw new ArgumentException(message: "Use valid connection string for Events database", paramName: nameof(EventDatabaseSettings.EventDatabaseConnectionString), exception);
			}
		}
	}

	public class EventDatabaseSettings
	{
		public const string SettingsName = nameof(EventDatabaseSettings);

		public string EventDatabaseConnectionString { get; set; }
		public string EventDatabaseName { get; set; }
		public string EventsCollectionName { get; set; }
	}
}