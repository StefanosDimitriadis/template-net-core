using Microsoft.Extensions.DependencyInjection;
using Template.Persistence.DatabaseContexts;

namespace Template.Persistence
{
	public static class ServiceExtensions
	{
		public static void AddDatabaseContexts(this IServiceCollection services)
		{
			services.AddBonusDatabase();
			services.AddCampaignDatabase();
			services.AddCustomerDatabase();
			services.AddEventDatabase();
		}
	}
}