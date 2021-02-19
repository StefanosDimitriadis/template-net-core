using Autofac;
using MediatR;
using Template.Application.Commands;
using Template.Application.Decorators;
using Template.Application.EventHandlers;
using Template.Application.Persistence;
using Template.Application.Queries;
using Template.Application.Services;

namespace Template.Application
{
	internal static class ContainerBuilderExtensions
	{
		internal static void RegisterDecorators(this ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(EventHandlingDecorator<>),
				serviceType: typeof(INotificationHandler<>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseEventHandler<,,>);
				});

			containerBuilder.RegisterGenericDecorator(typeof(EntityRetrievalPersistenceCacheDecorator<,>), typeof(IEntityRetrievalPersistence<,>));

			containerBuilder.RegisterGenericDecorator(typeof(EntityModificationCacheDecorator<,,,>), typeof(IEntityModificationPersistence<,,,>));

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(LogQueryDecorator<,>),
				serviceType: typeof(IRequestHandler<,>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseQueryHandler<,,>);
				});

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(LogCommandDecorator<,>),
				serviceType: typeof(IRequestHandler<,>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseCommandHandler<,,>);
				});

			containerBuilder.RegisterMetricsDecorators();
		}
	}
}