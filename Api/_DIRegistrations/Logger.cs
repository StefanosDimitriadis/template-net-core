using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Template.Api.DIRegistrations
{
	internal static class LoggerDIRegistration
	{
		internal static void UseLogging(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseNLog();
		}

		internal static void AddLoggingServices(this IServiceCollection services)
		{
			services.AddLogging(_loggingBuilder =>
			{
				_loggingBuilder.ClearProviders();
				_loggingBuilder.AddNLog(configFileName: "nlog.config");
			});
		}
	}
}