using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Values;
using Template.Persistence.DatabaseContexts;
using Template.Persistence.Settings;
using Template.Persistence.Storages;

namespace Template.Persistence.Tests.Storages
{
	public static class CampaignStorageHostBuilderFactory
	{
		public static IHostBuilder Create()
		{
			var hostBuilder = Host.CreateDefaultBuilder();
			hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
			hostBuilder.ConfigureHostConfiguration(_configurationBuilder => _configurationBuilder.AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true));
			hostBuilder.ConfigureServices((_hostBuilderContext, _services) =>
			{
				_services.Configure<DatabaseContextSettings>(_hostBuilderContext.Configuration.GetSection(DatabaseContextSettings.SettingsName));
				_services.AddSingleton(_serviceProvider => _serviceProvider.GetRequiredService<IOptions<DatabaseContextSettings>>().Value);
				_services.AddDbContext<CampaignDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
				{
					var serviceScope = _serviceProvider.CreateScope();
					var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
					_dbContextOptionsBuilder.UseSqlServer($"{databaseContextSettings.CampaignDatabaseConnectionString}");
				});
			});
			hostBuilder.ConfigureContainer<ContainerBuilder>(_containerBuilder =>
			{
				_containerBuilder.RegisterType<CampaignCommandStorage>().As<ICampaignCommandStorage>().SingleInstance();
				_containerBuilder.RegisterType<CampaignQueryStorage>().As<ICampaignQueryStorage>().SingleInstance();
			});
			return hostBuilder;
		}

		public static IHostBuilder CreateWithWrongConnectionString()
		{
			var hostBuilder = Host.CreateDefaultBuilder();
			hostBuilder.ConfigureHostConfiguration(_configurationBuilder => _configurationBuilder.AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true));
			hostBuilder.ConfigureServices((_hostBuilderContext, _services) =>
			{
				_services.Configure<DatabaseContextSettings>(_hostBuilderContext.Configuration.GetSection(DatabaseContextSettings.SettingsName));
				_services.AddSingleton(_serviceProvider => _serviceProvider.GetRequiredService<IOptions<DatabaseContextSettings>>().Value);
				_services.AddDbContext<CampaignDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
				{
					var serviceScope = _serviceProvider.CreateScope();
					var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
					_dbContextOptionsBuilder.UseSqlServer($"{databaseContextSettings.CampaignDatabaseConnectionString}+1");
				});
			});
			return hostBuilder;
		}
	}

	public static class CampaignFactory
	{
		public static Campaign Create(decimal bonusMoney)
		{
			return Campaign.Create(new CampaignSpecification(bonusMoney)).Entity;
		}
	}
}