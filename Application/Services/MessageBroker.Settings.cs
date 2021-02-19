using System;

namespace Template.Application.Services
{
	public class MessageBrokerSettings : IValidatable
	{
		public const string SettingsName = nameof(MessageBrokerSettings);

		public string Host { get; set; }
		public ushort Port { get; set; }
		public string VirtualHost { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string QueueName { get; set; }

		public void Validate()
		{
			if (string.IsNullOrWhiteSpace(Host))
				throw new ArgumentException(message: "Host cannot be null", paramName: nameof(Host));

			if (Port < 1)
				throw new ArgumentException(message: "Port cannot be negative", paramName: nameof(Port));

			if (string.IsNullOrWhiteSpace(VirtualHost))
				throw new ArgumentException(message: "Virtual host cannot be null", paramName: nameof(VirtualHost));

			if (string.IsNullOrWhiteSpace(Username))
				throw new ArgumentException(message: "Username cannot be null", paramName: nameof(Username));

			if (string.IsNullOrWhiteSpace(Password))
				throw new ArgumentException(message: "QueueName cannot be null", paramName: nameof(Password));

			if (string.IsNullOrWhiteSpace(QueueName))
				throw new ArgumentException(message: "Queue name cannot be null", paramName: nameof(QueueName));
		}
	}
}