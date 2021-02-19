using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Services;

namespace Template.Api.HealthChecks
{
	internal class MessageBrokerHealthCheck : IHealthCheck
	{
		private readonly MessageBrokerSettings _messageBrokerSettings;
		internal const string Name = "Message Broker health check";

		public MessageBrokerHealthCheck(MessageBrokerSettings messageBrokerSettings)
		{
			_messageBrokerSettings = messageBrokerSettings;
		}

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext healthCheckContext, CancellationToken cancellationToken)
		{
			const string message = "Checking message broker connection";
			try
			{
				var connectionFactory = new ConnectionFactory
				{
					Uri = new Uri($"amqp://{_messageBrokerSettings.Username}:{_messageBrokerSettings.Password}@{_messageBrokerSettings.Host}{_messageBrokerSettings.VirtualHost}")
				};
				var connection = connectionFactory.CreateConnection();
				using var model = connection.CreateModel();
				return Task.FromResult(model.IsOpen
					? HealthCheckResult.Healthy(message)
					: HealthCheckResult.Unhealthy(message));
			}
			catch (Exception exception)
			{
				return Task.FromResult(HealthCheckResult.Unhealthy(message, exception));
			}
		}
	}
}