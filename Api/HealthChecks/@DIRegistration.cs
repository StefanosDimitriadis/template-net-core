using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using Template.Api.HealthChecks.Publishers;

namespace Template.Api.HealthChecks
{
	public static class DIRegistration
	{
		internal static void AddHealthCheckServices(this IServiceCollection services)
		{
			services.AddHealthChecks()
				.AddCheck<MemoryHealthCheck>(MemoryHealthCheck.Name)
				.AddCheck<BonusDatabaseHealthCheck>(BonusDatabaseHealthCheck.Name)
				.AddCheck<CampaignDatabaseHealthCheck>(CampaignDatabaseHealthCheck.Name)
				.AddCheck<CustomerDatabaseHealthCheck>(CustomerDatabaseHealthCheck.Name)
				.AddCheck<EventDatabaseHealthCheck>(EventDatabaseHealthCheck.Name)
				.AddCheck<MessageBrokerHealthCheck>(MessageBrokerHealthCheck.Name);
			services.AddSingleton<IHealthCheckPublisher, LogHealthCheckPublisher>();
		}

		internal static void AddHealthCheckUiServices(this IServiceCollection services)
		{
			var serviceScope = services.BuildServiceProvider().CreateScope();
			var healthCheckSettings = serviceScope.ServiceProvider.GetRequiredService<HealthCheckSettings>();
			var assemblyPath = Assembly.GetExecutingAssembly().Location;
			var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
			var healthChecksUIStorageConnectionStringTemplate = healthCheckSettings.StorageConnectionString;
			var healthChecksUIStorageConnectionString = string.Format(healthChecksUIStorageConnectionStringTemplate, assemblyDirectory);
			services.AddHealthChecksUI(_settings =>
			{
				_settings.SetEvaluationTimeInSeconds(healthCheckSettings.PollingIntervalInSeconds);
				_settings.AddHealthCheckEndpoint(healthCheckSettings.HealthChecks[0].Name, healthCheckSettings.HealthChecks[0].Uri);
			})
				.AddSqliteStorage(healthChecksUIStorageConnectionString);
		}

		internal static void MapHealthChecks(this IEndpointRouteBuilder endpointRouteBuilder)
		{
			var healthCheckSettings = endpointRouteBuilder.ServiceProvider.GetRequiredService<HealthCheckSettings>();
			endpointRouteBuilder.MapHealthChecks(
				healthCheckSettings.HealthChecks[0].Uri,
				new HealthCheckOptions
				{
					ResponseWriter = async (_httpContext, _healthReport) =>
					{
						var content = JsonConvert.SerializeObject(_healthReport, Formatting.Indented, new StringEnumConverter());
						_httpContext.Response.ContentType = MediaTypeNames.Application.Json;
						_httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
						await _httpContext.Response.WriteAsync(content);
					}
				});
			endpointRouteBuilder.MapHealthChecksUI(_options =>
			{
				_options.UIPath = healthCheckSettings.UiUri;
			});
		}
	}
}