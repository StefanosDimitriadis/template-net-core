using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using System;
using System.Threading.Tasks;

namespace Template.Api
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			//TODO Testing
			var host = default(IHost);
			var logger = LogManager.GetCurrentClassLogger();
			try
			{
				var hostBuilder = Host.CreateDefaultBuilder(args);
				hostBuilder.UseNLog();
				logger.Info("Application starting");
				hostBuilder.UseAutofacServiceProviderFactory();
				hostBuilder.ConfigureWebHost();
				hostBuilder.ConfigureAppMetricsHosting();
				host = hostBuilder.Build();
				await host.RunAsync();
			}
			catch (Exception exception)
			{
				logger.Fatal(exception, "Application did not start");
				if (host != null)
					await host.StopAsync();

				throw;
			}
			finally
			{
				LogManager.Shutdown();
			}
		}
	}
}