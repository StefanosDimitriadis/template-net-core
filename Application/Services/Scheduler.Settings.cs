using System;
using System.Data.Common;

namespace Template.Application.Services
{
	public class SchedulerSettings : IValidatable
	{
		public const string SettingsName = nameof(SchedulerSettings);

		public string Endpoint { get; set; }
		public string StorageConnectionString { get; set; }
		public string DashboardTitle { get; set; }

		public void Validate()
		{
			if (string.IsNullOrWhiteSpace(Endpoint))
				throw new ArgumentException(message: "Scheduler endpoint cannot be null", paramName: nameof(Endpoint));

			if (!Endpoint.StartsWith("/"))
				throw new ArgumentException(message: "Scheduler endpoint must start with \"/\"", paramName: nameof(Endpoint));

			try
			{
				var dbConnectionStringBuilder = new DbConnectionStringBuilder
				{
					ConnectionString = StorageConnectionString
				};
			}
			catch (Exception exception)
			{
				throw new ArgumentException(message: "Use valid connection string for scheduler storage", paramName: nameof(StorageConnectionString), exception);
			}

			if (string.IsNullOrWhiteSpace(DashboardTitle))
				throw new ArgumentException(message: "Scheduler dashboard title cannot be null", paramName: nameof(DashboardTitle));
		}
	}
}