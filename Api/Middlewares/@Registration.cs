using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Template.Api.HealthChecks;
using Template.Application;
using Template.Infrastructure;

namespace Template.Api.Middlewares
{
	internal static class Registration
	{
		internal static void UseMiddlewares(this IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.UseMiddleware<LoggingMiddleware>();
			applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
			applicationBuilder.UseMiddleware<HttpRequestCountingMiddleware>();
			applicationBuilder.UseMiddleware<ExtraLogInfoWeavingMiddleware>();
			applicationBuilder.UseMiddleware<HttpRequestElapsedTimeMeasuringMiddleware>();
			applicationBuilder.UseRouting();
			applicationBuilder.UseMetricsMiddlewares();
			applicationBuilder.UseEndpoints(_endpointRouteBuilder =>
			{
				_endpointRouteBuilder.MapControllers();
				_endpointRouteBuilder.MapHealthChecks();
			});
			applicationBuilder.UseSwaggerMiddleware();
			applicationBuilder.UseHangfireMiddleware();
		}

		private static void UseSwaggerMiddleware(this IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.UseOpenApi();
			applicationBuilder.UseSwaggerUi3(_swaggerUi3Settings =>
			{
				var swaggerSettings = applicationBuilder.ApplicationServices.GetRequiredService<SwaggerSettings>();
				_swaggerUi3Settings.Path = swaggerSettings.Endpoint;
				_swaggerUi3Settings.DocumentTitle = "Api Documentation";
			});
		}
	}
}