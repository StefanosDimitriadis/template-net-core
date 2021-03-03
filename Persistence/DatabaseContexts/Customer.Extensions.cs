using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Persistence.Settings;

namespace Template.Persistence.DatabaseContexts
{
	public static class CustomerExtensions
	{
		public static void AddCustomerDatabase(this IServiceCollection services)
		{
			services.AddDbContext<CustomerDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
			{
				var serviceScope = _serviceProvider.CreateScope();
				var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
				_dbContextOptionsBuilder.UseSqlServer(
					databaseContextSettings.CustomerDatabaseConnectionString,
					_sqlServerDatabaseOptionsBuilderContext => _sqlServerDatabaseOptionsBuilderContext.CommandTimeout(databaseContextSettings.CustomerDatabaseTimeoutInSeconds));
			});
		}
	}
}