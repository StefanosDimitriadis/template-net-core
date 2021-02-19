using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using Template.Api.Filters;
using Template.Api.HealthChecks;
using Template.Api.Middlewares;
using Template.Application.Services;
using Template.Persistence.Settings;

namespace Template.Api.DIRegistrations
{
	internal static class SettingsDIRegistration
	{
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
	}
}