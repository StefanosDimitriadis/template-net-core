using Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using Template.Application.Services;
using Template.Domain.Entities;

namespace Template.Infrastructure
{
	internal static class SerializationExtensions
	{
		internal static byte[] ToBytes<TId, TEntity>(this TEntity entity)
			where TEntity : BaseEntity<TId>
		{
			var payload = JsonConvert.SerializeObject(entity);
			return Encoding.Unicode.GetBytes(payload);
		}

		internal static TEntity ToEntity<TId, TEntity>(this byte[] bytes)
			where TEntity : BaseEntity<TId>
		{
			var payload = Encoding.Unicode.GetString(bytes);
			return JsonConvert.DeserializeObject<TEntity>(
				value: payload,
				settings: new CustomJsonSerializerSettings());
		}
	}

	public class CustomJsonSerializerSettings : JsonSerializerSettings
	{
		public CustomJsonSerializerSettings()
		{
			Error = (object sender, ErrorEventArgs args) =>
			{
				if (args.CurrentObject == args.ErrorContext.OriginalObject)
				{
					var logger = LogManager.GetCurrentClassLogger(projectName: nameof(Infrastructure), className: nameof(CustomJsonSerializerSettings));
					logger.Error(args.ErrorContext.Error, $"Error in serializer for {args.ErrorContext.Member} at {args.ErrorContext.Path}");
				}
			};
		}
	}

	public static class ServiceExtensions
	{
		public static void AddMessageBrokerServices(this IServiceCollection services)
		{
			services.AddMassTransit(_serviceCollectionBusConfigurator =>
			{
				_serviceCollectionBusConfigurator.AddConsumers(typeof(BaseMessageBrokerConsumer<>).Assembly);
				_serviceCollectionBusConfigurator.UsingRabbitMq((_busRegistrationContext, _rabbitMqBusFactoryConfigurator) =>
				{
					_rabbitMqBusFactoryConfigurator.UseConsumeFilter(typeof(MessageBrokerMetricFilter<>), _busRegistrationContext);
					var serviceScope = _busRegistrationContext.CreateScope();
					var messageBrokerSettings = serviceScope.ServiceProvider.GetRequiredService<MessageBrokerSettings>();
					_rabbitMqBusFactoryConfigurator.Host(
						messageBrokerSettings.Host,
						messageBrokerSettings.Port,
						messageBrokerSettings.VirtualHost,
						_rabbitMqHostConfigurator =>
						{
							_rabbitMqHostConfigurator.Username(messageBrokerSettings.Username);
							_rabbitMqHostConfigurator.Password(messageBrokerSettings.Password);
						});
					_rabbitMqBusFactoryConfigurator.ReceiveEndpoint(messageBrokerSettings.QueueName, _receiveEndpointConfigurator =>
					{
						_receiveEndpointConfigurator.ConfigureConsumers(_busRegistrationContext);
					});
				});
			});
		}

		public static void AddSchedulerServices(this IServiceCollection services)
		{
			services.AddHangfire((_serviceProvider, _globalConfiguration) =>
			{
				var schedulerSettings = _serviceProvider.GetRequiredService<SchedulerSettings>();
				_globalConfiguration.UseSqlServerStorage(schedulerSettings.StorageConnectionString);
			});
			services.AddHangfireServer(_backgroundJobServerOptions =>
			{
				_backgroundJobServerOptions.ServerName = "Scheduler Server";
			});
		}
	}

	public static class ApplicationBuilderExtensions
	{
		public static void UseHangfireMiddleware(this IApplicationBuilder applicationBuilder)
		{
			var schedulerSettings = applicationBuilder.ApplicationServices.GetRequiredService<SchedulerSettings>();
			var dashboardOptions = new DashboardOptions
			{
				AppPath = default,
				DashboardTitle = schedulerSettings.DashboardTitle,
				DisplayStorageConnectionString = false,
				IsReadOnlyFunc = _dashboardContext =>
				{
					return true;
				}
			};
			applicationBuilder.UseHangfireDashboard(schedulerSettings.Endpoint, dashboardOptions);
		}
	}
}