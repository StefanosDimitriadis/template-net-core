using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Template.Api.ActionResults;
using Template.Application.Services;
using Template.Infrastructure;
using Template.Shared;

namespace Template.Api
{
	internal static class ExtensionMethods
	{
		internal static bool ContainsAny(this string value, string[] possibleValues)
		{
			foreach (string possibleValue in possibleValues)
				if (value.Contains(possibleValue))
					return true;

			return false;
		}

		internal static async Task WriteErrorResponse(this HttpContext httpContext, IEnumerable<Shared.Error> errors)
		{
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
	}

	internal static class HostBuilderExtensions
	{
		internal static void ConfigureWebHost(this IHostBuilder hostBuilder)
		{
			hostBuilder.ConfigureWebHostDefaults(_webHostBuilder =>
			{
				_webHostBuilder.UseStartup<Startup>();
			});
		}
	}

	internal static class ServiceExtensions
	{
		internal static void AddControllerServices(this IServiceCollection services)
		{
			services.AddControllers()
				.ConfigureApiBehaviorOptions(_apiBehaviorOptions => _apiBehaviorOptions.InvalidModelStateResponseFactory = _actionContext =>
				 {
					 return new ErrorActionResult();
				 })
				.AddNewtonsoftJson(_mvcNewtonsoftJsonOptions =>
				{
					_mvcNewtonsoftJsonOptions.SerializerSettings.Formatting = Formatting.Indented;
					_mvcNewtonsoftJsonOptions.SerializerSettings.ContractResolver = new CamelCasePropertyNamesExcludingEmptyErrorsContractResolver();
					_mvcNewtonsoftJsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					_mvcNewtonsoftJsonOptions.SerializerSettings.Error = (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) =>
					{
						if (args.CurrentObject == args.ErrorContext.OriginalObject)
						{
							var logger = LogManager.GetCurrentClassLogger();
							logger.Error(args.ErrorContext.Error, $"Error in serializer for {args.ErrorContext.Member} at {args.ErrorContext.Path}");
						}
					};
				})
				.AddFluentValidation(_fluentValidationMvcConfiguration =>
				{
					_fluentValidationMvcConfiguration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
					_fluentValidationMvcConfiguration.RegisterValidatorsFromAssemblyContaining(typeof(BaseValidator<>));
				});
		}

		internal class CamelCasePropertyNamesExcludingEmptyErrorsContractResolver : CamelCasePropertyNamesContractResolver
		{
			protected override JsonProperty CreateProperty(MemberInfo memberInfo, MemberSerialization memberSerialization)
			{
				var jsonProperty = base.CreateProperty(memberInfo, memberSerialization);
				if (memberInfo.Name == "Errors")
				{
					jsonProperty.ShouldSerialize = _instance =>
					{
						return (_instance?.GetType().GetProperty("Errors").GetValue(_instance) as IEnumerable<object>)?.Count() > 0;
					};
				}
				return jsonProperty;
			}
		}
	}
}