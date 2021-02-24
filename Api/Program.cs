using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
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
		private static ILogger _logger;

		public static async Task Main(string[] args)
		{
			//TODO Testing
			var host = default(IHost);
			var loggingConfiguration = ConfigureLogging();
			_logger = loggingConfiguration.CreateLogger();
			try
			{
				var hostBuilder = Host.CreateDefaultBuilder(args);
				hostBuilder.UseLogging();
				_logger?.Information("Application starting");
				hostBuilder.UseAutofacServiceProviderFactory();
				hostBuilder.ConfigureWebHost();
				hostBuilder.ConfigureMetricsHosting();
				host = hostBuilder.Build();
				await host.RunAsync();
			}
			catch (Exception exception)
			{
				_logger?.Fatal(exception, "Application did not start");
				if (host != null)
					await host.StopAsync();

				throw;
			}
			finally
			{
				LogManager.Shutdown();
			}
		}

		private static LoggerConfiguration ConfigureLogging()
		{
			//< layout xsi: type = "SimpleLayout" text = "${basicLayout} ${correlationIdLayout} " />
			//		< layout xsi: type = "JsonLayout" >
			//		< attribute name = "message" layout = "${message}" />
			//		</ layout >
			//< layout xsi: type = "SimpleLayout" text = " ${exceptionLayout}${newline}" />

			//< variable xsi: type = "NLogVariable" name = "longDate" value = "${longdate:universalTime=true}" />
			//< variable xsi: type = "NLogVariable" name = "basicLayout" value = "${longDate} ## ${level:uppercase=true} ## ${logger} Line: ${callsite-linenumber}" />
			//< variable xsi: type = "NLogVariable" name = "correlationIdLayout" value = "${when:when=length('${aspnet-TraceIdentifier:ignoreActivityId=true}') > 0:## CorrelationId\: ${aspnet-TraceIdentifier:ignoreActivityId=true}}" />
			//< variable xsi: type = "NLogVariable" name = "exceptionLayout" value = "${when:when=length('${exception}') > 0:## Exception\: ${exception:format=@:innerFormat=@:maxInnerExceptionLevel=10}}" />

			//var configurationBuilder = CreateApplicationConfigurationBuilder();
			//var configuration = configurationBuilder.Build();
			//TODO use utc?
			return new LoggerConfiguration()
				.WriteTo.Console(LogEventLevel.Verbose, outputTemplate: "{Timestamp:yyyy-MM-dd HH:MM:SS.mmm} ## {Level:u} ## {SourceContext} {Message:lj} {Exception}");
				//.WriteTo.File("./logs/test.log")
				//.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
				//{
				//	IndexFormat = "custom-index"
				//});
			//return new LoggerConfiguration().ReadFrom.Configuration(configuration, "LoggingSettings");
		}

		private static IConfigurationBuilder CreateApplicationConfigurationBuilder()
		{
			return new ConfigurationBuilder().AddJsonFile(_jsonConfigurationSource =>
			{
				_jsonConfigurationSource.Optional = false;
				_jsonConfigurationSource.Path = "appsettings.json";
				_jsonConfigurationSource.ReloadOnChange = true;
				_jsonConfigurationSource.OnLoadException = _fileLoadExceptionContext =>
				{
					_logger?.Error(_fileLoadExceptionContext.Exception, _fileLoadExceptionContext.Exception.Message);
				};
			});
		}
	}
}