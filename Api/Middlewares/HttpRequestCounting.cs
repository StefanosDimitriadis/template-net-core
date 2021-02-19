//using App.Metrics;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Template.Api.HealthChecks;
using Template.Application.Services;

namespace Template.Api.Middlewares
{
	internal class HttpRequestCountingMiddleware
	{
		private readonly RequestDelegate _nextRequestDelegate;
		//private readonly IMetrics _metrics;
		private readonly string[] _customMiddlewareEndpoints;

		public HttpRequestCountingMiddleware(
			RequestDelegate nextRequestDelegate,
			//IMetrics metrics,
			SwaggerSettings swaggerSettings,
			SchedulerSettings schedulerSettings,
			HealthCheckSettings healthCheckSettings)
		{
			_nextRequestDelegate = nextRequestDelegate;
			//_metrics = metrics;
			_customMiddlewareEndpoints = new string[]
			{
				Endpoints.Metrics,
				Endpoints.MetricsText,
				swaggerSettings.Endpoint,
				schedulerSettings.Endpoint,
				healthCheckSettings.HealthChecks[0].Uri,
				healthCheckSettings.UiUri
			};
		}

		public async Task Invoke(HttpContext httpContext)
		{
			var requestPath = httpContext.Request.Path.Value;
			var isCustomMiddlewareEndpoint = default(bool);
			if (requestPath.ContainsAny(_customMiddlewareEndpoints))
				isCustomMiddlewareEndpoint = true;

			try
			{
				//if (isCustomMiddlewareEndpoint)
				//	_metrics.Measure.Counter.Increment(ApiMetricsRegistry.HttpRequestCustomMiddlewareCounter);
				//else
				//	_metrics.Measure.Counter.Increment(ApiMetricsRegistry.HttpRequestTotalCounter);

				await _nextRequestDelegate(httpContext);
				//if (isCustomMiddlewareEndpoint)
				//	_metrics.Measure.Counter.Increment(ApiMetricsRegistry.HttpRequestCustomMiddlewareSuccessfulHandlingCounter);
				//else
				//	_metrics.Measure.Counter.Increment(ApiMetricsRegistry.HttpRequestSuccessfulHandlingCounter);
			}
			catch (Exception)
			{
				//if (isCustomMiddlewareEndpoint)
				//	_metrics.Measure.Counter.Increment(ApiMetricsRegistry.HttpRequestCustomMiddlewareUnsuccessfulHandlingCounter);
				//else
				//	_metrics.Measure.Counter.Increment(ApiMetricsRegistry.HttpRequestUnsuccessfulHandlingCounter);
				throw;
			}
		}
	}
}