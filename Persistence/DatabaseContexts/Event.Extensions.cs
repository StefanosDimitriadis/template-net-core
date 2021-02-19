using Microsoft.Extensions.DependencyInjection;

namespace Template.Persistence.DatabaseContexts
{
	public static class EventExtensions
	{
		public static void AddEventDatabase(this IServiceCollection services)
		{
			services.AddSingleton<EventDatabaseContext<long, long>>();
		}
	}
}