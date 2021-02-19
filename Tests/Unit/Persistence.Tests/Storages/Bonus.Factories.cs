using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Bonuses;
using Template.Persistence.DatabaseContexts;
using Template.Persistence.Settings;
using Template.Persistence.Storages;

namespace Template.Persistence.Tests.Storages
{
	public static class BonusStorageHostBuilderFactory
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
				_services.AddDbContext<BonusDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
				{
					var serviceScope = _serviceProvider.CreateScope();
					var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
					_dbContextOptionsBuilder.UseSqlServer($"{databaseContextSettings.BonusDatabaseConnectionString}");
				});
				_services.AddDbContext<CampaignDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
				{
					var serviceScope = _serviceProvider.CreateScope();
					var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
					_dbContextOptionsBuilder.UseSqlServer(databaseContextSettings.CampaignDatabaseConnectionString);
				});
				_services.AddDbContext<CustomerDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
				{
					var serviceScope = _serviceProvider.CreateScope();
					var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
					_dbContextOptionsBuilder.UseSqlServer(databaseContextSettings.CustomerDatabaseConnectionString);
				});
			});
			hostBuilder.ConfigureContainer<ContainerBuilder>(_containerBuilder =>
			{
				_containerBuilder.RegisterType<BonusCommandStorage>().As<IBonusCommandStorage>().SingleInstance();
				_containerBuilder.RegisterType<BonusQueryStorage>().As<IBonusQueryStorage>().SingleInstance();
				_containerBuilder.RegisterType<CampaignCommandStorage>().As<ICampaignCommandStorage>().SingleInstance();
				_containerBuilder.RegisterType<CampaignQueryStorage>().As<ICampaignQueryStorage>().SingleInstance();
				_containerBuilder.RegisterType<CustomerCommandStorage>().As<ICustomerCommandStorage>().SingleInstance();
				_containerBuilder.RegisterType<CustomerQueryStorage>().As<ICustomerQueryStorage>().SingleInstance();
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
				_services.AddDbContext<BonusDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
				{
					var serviceScope = _serviceProvider.CreateScope();
					var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
					_dbContextOptionsBuilder.UseSqlServer($"{databaseContextSettings.BonusDatabaseConnectionString}+1");
				});
			});
			return hostBuilder;
		}
	}

	public static class BonusFactory
	{
		public static Bonus CreateWithExistingCampaignAndCustomer(long existingCampaignId, long existingCustomerId)
		{
			return Bonus.Create(existingCampaignId, existingCustomerId).Entity;
		}

		public static Bonus CreateWithNonExistingCampaignOrCustomer()
		{
			return Bonus.Create(0, 0).Entity;
		}
	}
}