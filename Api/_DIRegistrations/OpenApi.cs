using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag;
using System.Net.Mime;

namespace Template.Api.DIRegistrations
{
	internal static class OpenApiDIRegistration
	{
		internal static void AddSwaggerServices(this IServiceCollection services)
		{
			services.AddSwaggerDocument(_aspNetCoreOpenApiDocumentGeneratorSettings =>
			{
				_aspNetCoreOpenApiDocumentGeneratorSettings.GenerateEnumMappingDescription = true;
				_aspNetCoreOpenApiDocumentGeneratorSettings.SchemaType = SchemaType.OpenApi3;
				_aspNetCoreOpenApiDocumentGeneratorSettings.DocumentName = "ApiDocumentation";
				_aspNetCoreOpenApiDocumentGeneratorSettings.PostProcess = _openApiDocument =>
				{
					_openApiDocument.Consumes = new string[]
					{
						MediaTypeNames.Application.Json
					};
					_openApiDocument.Info = new OpenApiInfo
					{
						Description = "Api Documentation",
						Title = "Api",
						Version = "1.0.0"
					};
					_openApiDocument.Produces = new string[]
					{
						MediaTypeNames.Application.Json
					};
				};
			});
		}
	}
}