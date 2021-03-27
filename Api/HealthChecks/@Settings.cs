using System;
using System.Data.Common;
using Template.Application.Services;

namespace Template.Api.HealthChecks
{
	internal class HealthCheckSettings : IValidatable
	{
		public const string SettingsName = "HealthChecksUI";

		public HealthCheck[] HealthChecks { get; set; }
		public string UiUri { get; set; }
		public ushort PollingIntervalInSeconds { get; set; }
		public string StorageConnectionString { get; set; }
		public MemoryHealthCheckSettings MemorySettings { get; set; }

		public void Validate()
		{
			if (HealthChecks == null || HealthChecks.Length == 0)
				throw new ArgumentException(message: "Health checks cannot be null or empty", paramName: nameof(HealthChecks));
			foreach (var healthCheck in HealthChecks)
			{
				if (string.IsNullOrWhiteSpace(healthCheck.Uri))
					throw new ArgumentException(message: "Uri for health check cannot be null", paramName: nameof(healthCheck.Uri));

				if (string.IsNullOrWhiteSpace(healthCheck.Name))
					throw new ArgumentException(message: "Name for health check cannot be null", paramName: nameof(healthCheck.Name));
			}

			if (string.IsNullOrWhiteSpace(UiUri))
				throw new ArgumentException(message: "Uri for health checks ui cannot be null", paramName: nameof(UiUri));

			if (PollingIntervalInSeconds < 1)
				throw new ArgumentException(message: "Polling interval cannot be negative", paramName: nameof(PollingIntervalInSeconds));

			try
			{
				var dbConnectionStringBuilder = new DbConnectionStringBuilder
				{
					ConnectionString = StorageConnectionString
				};
			}
			catch (Exception exception)
			{
				throw new ArgumentException(message: "Use valid connection string for health checks storage", paramName: nameof(StorageConnectionString), exception);
			}

			if (MemorySettings.MemoryLimitInMBs < 1)
				throw new ArgumentException(message: "Memory limit cannot be negative", paramName: nameof(MemorySettings.MemoryLimitInMBs));
		}
	}

	internal class HealthCheck
	{
		public string Name { get; set; }
		public string Uri { get; set; }
	}
}