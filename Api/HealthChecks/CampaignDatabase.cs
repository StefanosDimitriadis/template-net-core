using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Persistence.DatabaseContexts;

namespace Template.Api.HealthChecks
{
	internal class CampaignDatabaseHealthCheck : IHealthCheck
	{
		private readonly CampaignDatabaseContext _campaignDatabaseContext;
		internal const string Name = "Campaign database health check";

		public CampaignDatabaseHealthCheck(CampaignDatabaseContext campaignDatabaseContext)
		{
			_campaignDatabaseContext = campaignDatabaseContext;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext healthCheckContext, CancellationToken cancellationToken)
		{
			const string message = "Checking Campaign database connection";
			var canConnect = await _campaignDatabaseContext.Database.CanConnectAsync(cancellationToken);
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