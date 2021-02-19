using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Template.Api.Middlewares
{
	internal class HttpRequestElapsedTimeMeasuringMiddleware
	{
		private readonly RequestDelegate _nextRequestDelegate;
		private readonly HttpRequestElapsedTimeMeasuringSettings _httpRequestElapsedTimeMeasuringSettings;
		private readonly IMetrics _metrics;
		private readonly ILogger<HttpRequestElapsedTimeMeasuringMiddleware> _logger;

		public HttpRequestElapsedTimeMeasuringMiddleware(
			RequestDelegate nextRequestDelegate,
			HttpRequestElapsedTimeMeasuringSettings httpRequestElapsedTimeMeasuringSettings,
			IMetrics metrics,
			ILoggerFactory loggerFactory)
		{
			_nextRequestDelegate = nextRequestDelegate;
			_httpRequestElapsedTimeMeasuringSettings = httpRequestElapsedTimeMeasuringSettings;
			_metrics = metrics;
			_logger = loggerFactory.CreateLogger<HttpRequestElapsedTimeMeasuringMiddleware>();
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				await _nextRequestDelegate(httpContext);
				stopwatch.Stop();
				var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
				var logLevel = elapsedMilliseconds > _httpRequestElapsedTimeMeasuringSettings.ElapsedTimeLimitInMilliseconds
					? LogLevel.Error
					: LogLevel.Information;
				_logger.Log(logLevel, $"Request took {elapsedMilliseconds} ms");
				_metrics.Measure.Timer.Time(ApiMetricsRegistry.HttpRequestHandlingTimer, elapsedMilliseconds);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}