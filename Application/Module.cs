using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace Template.Application
{
	public class ApplicationModule : Module
	{
		protected override void Load(ContainerBuilder containerBuilder)
		{
			var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			containerBuilder.RegisterMediatR(executingAssembly);
			containerBuilder.RegisterDecorators();
		}
	}
}