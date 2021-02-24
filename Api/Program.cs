using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Template.Api.DIRegistrations;
using Template.Api.HostBuilder.DIRegistrations;
using Template.Application;
using Template.Application.Services;

namespace Template.Api
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			//TODO Testing
			var host = default(IHost);
			var logger = LogManager.GetCurrentClassLogger(projectName: nameof(Api), className: nameof(Program));
			try
			{
				var hostBuilder = Host.CreateDefaultBuilder(args);
				hostBuilder.UseLogging();
				logger.Info("Application starting");
				hostBuilder.UseAutofacServiceProviderFactory();
				hostBuilder.ConfigureWebHost();
				hostBuilder.ConfigureMetricsHosting();
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