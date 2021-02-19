using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;

namespace Template.Api
{
	internal class ApiModule : Module
	{
		protected override void Load(ContainerBuilder containerBuilder)
		{
			var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			containerBuilder.RegisterAutoMapper(executingAssembly);
			containerBuilder.RegisterDecorators();
		}
	}
}