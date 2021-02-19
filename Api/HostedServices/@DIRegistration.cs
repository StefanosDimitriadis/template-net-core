using Microsoft.Extensions.DependencyInjection;

namespace Template.Api.HostedServices
{
	internal static class DIRegistration
	{
		internal static void AddHostedServices(this IServiceCollection services)
		{
			services.AddHostedService<CampaignDatabaseInitializationHostedService>();
			services.AddHostedService<CustomerDatabaseInitializationHostedService>();
			services.AddHostedService<ScheduleFirstOfMonthCustomerNotificationHostedService>();
		}
	}
}