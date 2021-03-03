using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Persistence.Settings;

namespace Template.Persistence.DatabaseContexts
{
	public static class CampaignExtensions
	{
		public static void AddCampaignDatabase(this IServiceCollection services)
		{
			services.AddDbContext<CampaignDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
			{
				var serviceScope = _serviceProvider.CreateScope();
				var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
				_dbContextOptionsBuilder.UseSqlServer(
					databaseContextSettings.CampaignDatabaseConnectionString,
					_sqlServerDatabaseOptionsBuilderContext => _sqlServerDatabaseOptionsBuilderContext.CommandTimeout(databaseContextSettings.CampaignDatabaseTimeoutInSeconds));
			});
		}
	}
}