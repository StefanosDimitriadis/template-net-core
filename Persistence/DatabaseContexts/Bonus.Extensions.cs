using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Persistence.Settings;

namespace Template.Persistence.DatabaseContexts
{
	public static class BonusExtensions
	{
		public static void AddBonusDatabase(this IServiceCollection services)
		{
			services.AddDbContext<BonusDatabaseContext>((_serviceProvider, _dbContextOptionsBuilder) =>
			{
				var serviceScope = _serviceProvider.CreateScope();
				var databaseContextSettings = serviceScope.ServiceProvider.GetRequiredService<DatabaseContextSettings>();
				_dbContextOptionsBuilder.UseSqlServer(databaseContextSettings.BonusDatabaseConnectionString);
			});
		}
	}
}