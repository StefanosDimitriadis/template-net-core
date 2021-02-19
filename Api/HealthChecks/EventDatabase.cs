using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Persistence.Settings;

namespace Template.Api.HealthChecks
{
	internal class EventDatabaseHealthCheck : IHealthCheck
	{
		private readonly DatabaseContextSettings _databaseContextSettings;
		internal const string Name = "Event database health check";

		public EventDatabaseHealthCheck(DatabaseContextSettings databaseContextSettings)
		{
			_databaseContextSettings = databaseContextSettings;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext healthCheckContext, CancellationToken cancellationToken)
		{
			const string message = "Checking Event database connection";
			var client = new MongoClient(_databaseContextSettings.EventDatabaseSettings.EventDatabaseConnectionString);
			var database = client.GetDatabase(_databaseContextSettings.EventDatabaseSettings.EventDatabaseName);
			var canConnect = default(bool);
			try
			{
				await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);
				canConnect = true;
			}
			catch (Exception) { }
			try
			{
				return canConnect
					? HealthCheckResult.Healthy(message)
					: HealthCheckResult.Unhealthy(message);
			}
			catch (Exception exception)
			{
				return HealthCheckResult.Unhealthy(message, exception);
			}
		}
	}
}