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
		public int BonusDatabaseTimeoutInSeconds { get; set; }
		public string CampaignDatabaseConnectionString { get; set; }
		public int CampaignDatabaseTimeoutInSeconds { get; set; }
		public string CustomerDatabaseConnectionString { get; set; }
		public int CustomerDatabaseTimeoutInSeconds { get; set; }
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

			if (BonusDatabaseTimeoutInSeconds < 1)
				throw new ArgumentException(message: "Bonus timeout should be greater than 0", paramName: nameof(BonusDatabaseTimeoutInSeconds));

			if (CampaignDatabaseTimeoutInSeconds < 1)
				throw new ArgumentException(message: "Campaign timeout should be greater than 0", paramName: nameof(CampaignDatabaseTimeoutInSeconds));

			if (CustomerDatabaseTimeoutInSeconds < 1)
				throw new ArgumentException(message: "Customer timeout should be greater than 0", paramName: nameof(CustomerDatabaseTimeoutInSeconds));

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