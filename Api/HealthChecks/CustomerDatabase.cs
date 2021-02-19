using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Persistence.DatabaseContexts;

namespace Template.Api.HealthChecks
{
	internal class CustomerDatabaseHealthCheck : IHealthCheck
	{
		private readonly CustomerDatabaseContext _customerDatabaseContext;
		internal const string Name = "Customer database health check";

		public CustomerDatabaseHealthCheck(CustomerDatabaseContext customerDatabaseContext)
		{
			_customerDatabaseContext = customerDatabaseContext;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext healthCheckContext, CancellationToken cancellationToken)
		{
			const string message = "Checking Customer database connection";
			var canConnect = await _customerDatabaseContext.Database.CanConnectAsync(cancellationToken);
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