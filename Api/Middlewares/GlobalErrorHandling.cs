using App.Metrics;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Template.Infrastructure;
using Template.Shared;

namespace Template.Api.Middlewares
{
	internal class GlobalErrorHandlingMiddleware
	{
		private readonly RequestDelegate _nextRequestDelegate;
		private readonly IMetrics _metrics;
		private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

		public GlobalErrorHandlingMiddleware(
			RequestDelegate nextRequestDelegate,
			IMetrics metrics,
			ILoggerFactory loggerFactory)
		{
			_nextRequestDelegate = nextRequestDelegate;
			_metrics = metrics;
			_logger = loggerFactory.CreateLogger<GlobalErrorHandlingMiddleware>();
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				httpContext.Response.ContentType = MediaTypeNames.Application.Json;
				httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
				await _nextRequestDelegate(httpContext);
			}
			catch (ValidationException validationException)
			{
				_metrics.Measure.Counter.Increment(ApiMetricsRegistry.ValidationExceptionCounter);
				_logger.LogError(validationException, "A validation error occurred");
				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				var errors = validationException.Errors.Select(_validationFailure => new Error(_validationFailure.ErrorMessage));
				var jsonSerializerSettings = new CustomJsonSerializerSettings
				{
					Formatting = Formatting.Indented,
					ContractResolver = new CamelCasePropertyNamesContractResolver(),
				};
				var response = JsonConvert.SerializeObject(
					new
					{
						errors
					},
					jsonSerializerSettings);
				await httpContext.Response.WriteAsync(response);
			}
			catch (Exception exception)
			{
				_metrics.Measure.Counter.Increment(ApiMetricsRegistry.ExceptionCounter);
				_logger.LogError(exception, "An error occurred");
				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				var errors = new Error[]
				{
					new Error("An error occurred")
				};
				await httpContext.WriteErrorResponse(errors);
			}
		}
	}
}