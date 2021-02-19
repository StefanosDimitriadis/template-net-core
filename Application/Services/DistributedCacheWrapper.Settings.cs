using System;

namespace Template.Application.Services
{
	public class DistributedCacheWrapperSettings : IValidatable
	{
		public const string SettingsName = nameof(DistributedCacheWrapperSettings);

		public string ConnectionString { get; set; }
		public double AbsoluteExpirationSeconds { get; set; }
		public double SlidingExpirationSeconds { get; set; }
		public bool UseRedis { get; set; }

		public void Validate()
		{
			if (UseRedis && string.IsNullOrWhiteSpace(ConnectionString))
				throw new ArgumentException(message: "Connection string for Redis cache cannot be null", paramName: nameof(ConnectionString));
		}
	}
}