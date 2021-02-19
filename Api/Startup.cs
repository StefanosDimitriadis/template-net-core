using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Api.DIRegistrations;
using Template.Api.HealthChecks;
using Template.Api.HostBuilder.DIRegistrations;
using Template.Api.HostedServices;
using Template.Api.Middlewares;
using Template.Application;
using Template.Infrastructure;
using Template.Persistence;

namespace Template.Api
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSettingConfigurations(_configuration);
			services.AddLoggingServices();
			services.AddControllerServices();
			services.AddSwaggerServices();
			services.AddDatabaseContexts();
			services.AddHostedServices();
			services.AddMessageBrokerServices();
			services.AddHealthCheckServices();
			services.AddHealthCheckUiServices();
			services.AddMetricServices();
			services.AddSchedulerServices();
		}

		public void Configure(IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.UseMiddlewares();
		}

		public void ConfigureContainer(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterModules();
		}
	}
}