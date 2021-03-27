using System;
using Template.Application.Services;

namespace Template.Api.Middlewares
{
	internal class SwaggerSettings : IValidatable
	{
		public const string SettingsName = nameof(SwaggerSettings);

		public string Endpoint { get; set; }

		public void Validate()
		{
			if (string.IsNullOrWhiteSpace(Endpoint))
				throw new ArgumentException(message: "Scheduler endpoint cannot be null", paramName: nameof(Endpoint));

			if (!Endpoint.StartsWith("/"))
				throw new ArgumentException(message: "Scheduler endpoint must start with \"/\"", paramName: nameof(Endpoint));
		}
	}
}