using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Template.Api.Mappings;
using Template.Application;
using Template.Infrastructure;
using Template.Persistence;

namespace Template.Api.HostBuilder.DIRegistrations
{
	internal static class AutofacDIRegistration
	{
		internal static void UseAutofacServiceProviderFactory(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
		}

		internal static void RegisterModules(this ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterModule<ApiModule>();
			containerBuilder.RegisterModule<ApplicationModule>();
			containerBuilder.RegisterModule<InfrastructureModule>();
			containerBuilder.RegisterModule<PersistenceModule>();
		}

		internal static void RegisterDecorators(this ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterMetricsDecorators();
		}

		private static void RegisterMetricsDecorators(this ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterDecorator<MappingMetricDecorator, IMapper>();
		}
	}
}