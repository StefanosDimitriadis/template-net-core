using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Api.HealthChecks
{
	internal class MemoryHealthCheck : IHealthCheck
	{
		private readonly MemoryHealthCheckSettings _memoryHealthCheckSettings;
		internal const string Name = "Memory health check";

		public MemoryHealthCheck(HealthCheckSettings healthCheckSettings)
		{
			_memoryHealthCheckSettings = healthCheckSettings.MemorySettings;
		}

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext healthCheckContext, CancellationToken cancellationToken)
		{
			var allocatedMemoryInMBs = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
			var memoryLimitInMBs = _memoryHealthCheckSettings.MemoryLimitInMBs;
			var message = $@"Checking memory allocation
							Current allocated memory: [{allocatedMemoryInMBs}] MBs";
			var errorMessage = $@"{message}
								Allocated memory limit: [{memoryLimitInMBs} MBs]";
			try
			{
				return Task.FromResult(allocatedMemoryInMBs < memoryLimitInMBs
					? HealthCheckResult.Healthy(message)
					: HealthCheckResult.Unhealthy(errorMessage));
			}
			catch (Exception exception)
			{
				return Task.FromResult(HealthCheckResult.Unhealthy(errorMessage, exception));
			}
		}
	}
}