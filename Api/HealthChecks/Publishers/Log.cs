using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Api.HealthChecks.Publishers
{
	internal class LogHealthCheckPublisher : IHealthCheckPublisher
	{
		private readonly ILogger<LogHealthCheckPublisher> _logger;

		public LogHealthCheckPublisher(ILogger<LogHealthCheckPublisher> logger)
		{
			_logger = logger;
		}

		public Task PublishAsync(HealthReport healthReport, CancellationToken cancellationToken)
		{
			var healthCheckReports = healthReport.Entries;
			switch (healthReport.Status)
			{
				case HealthStatus.Unhealthy:
					var healthCheckErrorData = healthCheckReports.Where(_healthReportEntry => _healthReportEntry.Value.Status == HealthStatus.Unhealthy);
					_logger.LogError(JsonConvert.SerializeObject(healthCheckErrorData, new StringEnumConverter()));
					break;
				case HealthStatus.Degraded:
				case HealthStatus.Healthy:
					var healthCheckNonErrorData = healthCheckReports.Where(_healthReportEntry => _healthReportEntry.Value.Status != HealthStatus.Unhealthy);
					_logger.LogInformation(JsonConvert.SerializeObject(healthCheckNonErrorData, new StringEnumConverter()));
					break;
			}
			return Task.CompletedTask;
		}
	}
}