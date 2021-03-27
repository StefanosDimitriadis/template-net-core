using System;
using Template.Application.Services;

namespace Template.Api.Middlewares
{
	internal class HttpRequestElapsedTimeMeasuringSettings : IValidatable
	{
		public const string SettingsName = nameof(HttpRequestElapsedTimeMeasuringSettings);

		public uint ElapsedTimeLimitInMilliseconds { get; set; }

		public void Validate()
		{
			if (ElapsedTimeLimitInMilliseconds < 1)
				throw new ArgumentException(message: "Elapsed time limit cannot be negative", paramName: nameof(ElapsedTimeLimitInMilliseconds));
		}
	}
}