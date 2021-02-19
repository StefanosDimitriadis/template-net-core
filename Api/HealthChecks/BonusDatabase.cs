using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Persistence.DatabaseContexts;

namespace Template.Api.HealthChecks
{
	internal class BonusDatabaseHealthCheck : IHealthCheck
	{
		private readonly BonusDatabaseContext _bonusDatabaseContext;
		internal const string Name = "Bonus database health check";

		public BonusDatabaseHealthCheck(BonusDatabaseContext bonusDatabaseContext)
		{
			_bonusDatabaseContext = bonusDatabaseContext;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext healthCheckContext, CancellationToken cancellationToken)
		{
			const string message = "Checking bonus database connection";
			var canConnect = await _bonusDatabaseContext.Database.CanConnectAsync(cancellationToken);
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