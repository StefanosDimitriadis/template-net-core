//using App.Metrics.AspNetCore;
//using App.Metrics.Formatters.Prometheus;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NLog;
using NLog.Web;
using NSwag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using Template.Api.ActionResults;
using Template.Api.Decorators;
using Template.Api.Filters;
using Template.Api.HealthChecks;
using Template.Api.HealthChecks.Publishers;
using Template.Api.HostedServices;
using Template.Api.Middlewares;
using Template.Application;
using Template.Application.Services;
using Template.Infrastructure;
using Template.Persistence;
using Template.Persistence.DatabaseContexts;
using Template.Persistence.Settings;
using Template.Shared;

namespace Template.Api
{
	internal static class Endpoints
	{
		internal const string Metrics = "/metrics";
		internal const string MetricsText = "/metrics-text";
	}

	internal static class ExtensionMethods
	{
		internal static bool ContainsAny(this string value, string[] possibleValues)
		{
			foreach (string possibleValue in possibleValues)
				if (value.Contains(possibleValue))
					return true;

			return false;
		}

		internal static async Task WriteErrorResponse(this HttpContext httpContext, IEnumerable<Shared.Error> errors)
		{
			var jsonSerializerSettings = new CustomJsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
			};
			var response = JsonConvert.SerializeObject(
				new
				{
					errors
				},
				jsonSerializerSettings);
			await httpContext.Response.WriteAsync(response);
		}
	}

	internal static class HostBuilderExtensions
	{
		internal static void UseAutofacServiceProviderFactory(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
		}

		internal static void ConfigureWebHost(this IHostBuilder hostBuilder)
		{
			hostBuilder.ConfigureWebHostDefaults(_webHostBuilder =>
			{
				_webHostBuilder.UseStartup<Startup>();
			});
		}

		internal static void ConfigureAppMetricsHosting(this IHostBuilder hostBuilder)
		{
			//hostBuilder.UseMetrics(_metricsWebHostOptions => _metricsWebHostOptions.EndpointOptions = _metricEndpointsOptions =>
			//{
			//	_metricEndpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
			//	_metricEndpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
			//	_metricEndpointsOptions.EnvironmentInfoEndpointEnabled = false;
			//});
			//hostBuilder.ConfigureAppMetricsHostingConfiguration(_metricsEndpointsHostingOptions =>
			//{
			//	_metricsEndpointsHostingOptions.MetricsEndpoint = Endpoints.Metrics;
			//	_metricsEndpointsHostingOptions.MetricsTextEndpoint = Endpoints.MetricsText;
			//});
		}
	}

	internal static class ServiceExtensions
	{
		internal static void AddLoggingServices(this IServiceCollection services)
		{
			services.AddLogging(_loggingBuilder =>
			{
				_loggingBuilder.ClearProviders();
				_loggingBuilder.AddNLog(configFileName: "nlog.config");
			});
		}

		internal static void AddSettingConfigurations(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<IStartupFilter, SettingsValidationStartupFilter>();
			services.Configure<KestrelServerOptions>(_kestrelServerOptions =>
			{
				_kestrelServerOptions.AllowSynchronousIO = true;
			});
			services.Configure<IISServerOptions>(_iisServerOptions =>
			{
				_iisServerOptions.AllowSynchronousIO = true;
			});
			services.ConfigureSettings<DatabaseContextSettings>(configuration, DatabaseContextSettings.SettingsName);
			services.ConfigureSettings<BonusDeletionNotifierSettings>(configuration, BonusDeletionNotifierSettings.SettingsName);
			services.ConfigureSettings<CustomerNotifierEmailSettings>(configuration, CustomerNotifierEmailSettings.SettingsName);
			services.ConfigureSettings<MessageBrokerSettings>(configuration, MessageBrokerSettings.SettingsName);
			services.ConfigureSettings<DistributedCacheWrapperSettings>(configuration, DistributedCacheWrapperSettings.SettingsName);
			services.ConfigureSettings<HealthCheckSettings>(configuration, HealthCheckSettings.SettingsName);
			services.ConfigureSettings<HttpRequestElapsedTimeMeasuringSettings>(configuration, HttpRequestElapsedTimeMeasuringSettings.SettingsName);
			services.ConfigureSettings<SwaggerSettings>(configuration, SwaggerSettings.SettingsName);
			services.ConfigureSettings<SchedulerSettings>(configuration, SchedulerSettings.SettingsName);
			services.ConfigureSettings<PolicyExecutorSettings>(configuration, PolicyExecutorSettings.SettingsName);
		}

		private static void ConfigureSettings<TSettings>(this IServiceCollection services, IConfiguration configuration, string settingsName)
			where TSettings : class, IValidatable
		{
			services.Configure<TSettings>(configuration.GetSection(settingsName));
			services.AddTransient(_serviceProvider => _serviceProvider.GetRequiredService<IOptionsMonitor<TSettings>>().CurrentValue);
			services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<TSettings>>().OnChange(_settings =>
			{
				try
				{
					_settings.Validate();
					services.Configure<TSettings>(configuration.GetSection(settingsName));
					services.AddTransient(_serviceProvider => _serviceProvider.GetRequiredService<IOptionsMonitor<TSettings>>().CurrentValue);
				}
				catch (Exception exception)
				{
					var logger = LogManager.GetCurrentClassLogger();
					logger.Error(exception, $"Exception raised while changing the application settings to [{JsonConvert.SerializeObject(_settings)}]");
					throw;
				}
			});
			services.AddTransient(_serviceProvider => (IValidatable)_serviceProvider.GetRequiredService<IOptionsMonitor<TSettings>>().CurrentValue);
		}

		internal static void AddControllerServices(this IServiceCollection services)
		{
			services.AddControllers()
				.ConfigureApiBehaviorOptions(_apiBehaviorOptions => _apiBehaviorOptions.InvalidModelStateResponseFactory = _actionContext =>
				 {
					 return new ErrorActionResult();
				 })
				.AddNewtonsoftJson(_mvcNewtonsoftJsonOptions =>
				{
					_mvcNewtonsoftJsonOptions.SerializerSettings.Formatting = Formatting.Indented;
					_mvcNewtonsoftJsonOptions.SerializerSettings.ContractResolver = new CamelCasePropertyNamesExcludingEmptyErrorsContractResolver();
					_mvcNewtonsoftJsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					_mvcNewtonsoftJsonOptions.SerializerSettings.Error = (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) =>
					{
						if (args.CurrentObject == args.ErrorContext.OriginalObject)
						{
							var logger = LogManager.GetCurrentClassLogger();
							logger.Error(args.ErrorContext.Error, $"Error in serializer for {args.ErrorContext.Member} at {args.ErrorContext.Path}");
						}
					};
				})
				.AddFluentValidation(_fluentValidationMvcConfiguration =>
				{
					_fluentValidationMvcConfiguration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
					_fluentValidationMvcConfiguration.RegisterValidatorsFromAssemblyContaining(typeof(BaseValidator<>));
				});
		}

		internal static void AddSwaggerServices(this IServiceCollection services)
		{
			services.AddSwaggerDocument(_aspNetCoreOpenApiDocumentGeneratorSettings =>
			{
				_aspNetCoreOpenApiDocumentGeneratorSettings.GenerateEnumMappingDescription = true;
				_aspNetCoreOpenApiDocumentGeneratorSettings.SchemaType = SchemaType.OpenApi3;
				_aspNetCoreOpenApiDocumentGeneratorSettings.DocumentName = "ApiDocumentation";
				_aspNetCoreOpenApiDocumentGeneratorSettings.GenerateEnumMappingDescription = true;
				_aspNetCoreOpenApiDocumentGeneratorSettings.PostProcess = _openApiDocument =>
				{
					_openApiDocument.Consumes = new string[]
					{
						MediaTypeNames.Application.Json
					};
					_openApiDocument.Info = new OpenApiInfo
					{
						Description = "Api Documentation",
						Title = "Api",
						Version = "1.0.0"
					};
					_openApiDocument.Produces = new string[]
					{
						MediaTypeNames.Application.Json
					};
				};
			});
		}

		internal static void AddDatabaseContexts(this IServiceCollection services)
		{
			services.AddDbContext<BonusDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
			{
				var serviceScope = _serviceProvider.CreateScope();
				var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
				_dbContextOptionsBuilder.UseSqlServer(databaseContextSettings.BonusDatabaseConnectionString);
			});
			services.AddDbContext<CampaignDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
			{
				var serviceScope = _serviceProvider.CreateScope();
				var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
				_dbContextOptionsBuilder.UseSqlServer(databaseContextSettings.CampaignDatabaseConnectionString);
			});
			services.AddDbContext<CustomerDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
			{
				var serviceScope = _serviceProvider.CreateScope();
				var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
				_dbContextOptionsBuilder.UseSqlServer(databaseContextSettings.CustomerDatabaseConnectionString);
			});
			services.AddSingleton<EventDatabaseContext<long, long>>();
		}

		internal static void AddHostedServices(this IServiceCollection services)
		{
			services.AddHostedService<CampaignDatabaseInitializationHostedService>();
			services.AddHostedService<CustomerDatabaseInitializationHostedService>();
			services.AddHostedService<ScheduleFirstOfMonthCustomerNotificationHostedService>();
		}

		internal static void AddMessageBrokerServices(this IServiceCollection services)
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

		internal static void AddMetricServices(this IServiceCollection services)
		{
			//services.AddMetrics();
			//services.AddMetricsEndpoints();
			//services.AddMetricsReportingHostedService((sender, eventArgs) =>
			//{
			//	var logger = LogManager.GetCurrentClassLogger();
			//	logger.Error(eventArgs.Exception);
			//});
			//services.AddMetricsTrackingMiddleware();
		}

		internal static void AddSchedulerServices(this IServiceCollection services)
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

	internal static class ApplicationBuilderExtensions
	{
		internal static void UseMiddlewares(this IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.UseMiddleware<LoggingMiddleware>();
			applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
			applicationBuilder.UseMiddleware<HttpRequestCountingMiddleware>();
			applicationBuilder.UseMiddleware<ExtraLogInfoWeavingMiddleware>();
			applicationBuilder.UseMiddleware<HttpRequestElapsedTimeMeasuringMiddleware>();
			applicationBuilder.UseRouting();
			//applicationBuilder.UseMetricsMiddlewares();
			applicationBuilder.UseEndpoints(_endpointRouteBuilder =>
			{
				_endpointRouteBuilder.MapControllers();
				_endpointRouteBuilder.MapHealthChecks();
			});
			applicationBuilder.UseSwaggerMiddleware();
			applicationBuilder.UseHangfireMiddleware();
		}

		private static void UseSwaggerMiddleware(this IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.UseOpenApi();
			applicationBuilder.UseSwaggerUi3(_swaggerUi3Settings =>
			{
				var swaggerSettings = applicationBuilder.ApplicationServices.GetRequiredService<SwaggerSettings>();
				_swaggerUi3Settings.Path = swaggerSettings.Endpoint;
				_swaggerUi3Settings.DocumentTitle = "Api Documentation";
			});
		}

		private static void UseMetricsMiddlewares(this IApplicationBuilder applicationBuilder)
		{
			//applicationBuilder.UseMetricsEndpoint();
			//applicationBuilder.UseMetricsTextEndpoint();
			//applicationBuilder.UseMetricsActiveRequestMiddleware();
			//applicationBuilder.UseMetricsApdexTrackingMiddleware();
			//applicationBuilder.UseMetricsErrorTrackingMiddleware();
			//applicationBuilder.UseMetricsOAuth2TrackingMiddleware();
			//applicationBuilder.UseMetricsPostAndPutSizeTrackingMiddleware();
			//applicationBuilder.UseMetricsRequestTrackingMiddleware();
		}

		private static void UseHangfireMiddleware(this IApplicationBuilder applicationBuilder)
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

	internal static class EndpointRouteBuilder
	{
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

	internal static class ContainerBuilderExtensions
	{
		internal static void RegisterModules(this ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterModule<ApiModule>();
			containerBuilder.RegisterModule<ApplicationModule>();
			containerBuilder.RegisterModule<InfrastructureModule>();
			containerBuilder.RegisterModule<PersistenceModule>();
		}

		internal static void RegisterDecorators(this ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterDecorator<MapperMetricDecorator, IMapper>();
		}
	}

	internal class CamelCasePropertyNamesExcludingEmptyErrorsContractResolver : CamelCasePropertyNamesContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo memberInfo, MemberSerialization memberSerialization)
		{
			var jsonProperty = base.CreateProperty(memberInfo, memberSerialization);
			if (memberInfo.Name == "Errors")
			{
				jsonProperty.ShouldSerialize = _instance =>
				{
					return (_instance?.GetType().GetProperty("Errors").GetValue(_instance) as IEnumerable<object>)?.Count() > 0;
				};
			}
			return jsonProperty;
		}
	}
}