using Autofac;
using MediatR;
using Template.Application.Commands;
using Template.Application.Commands.Customers;
using Template.Application.Decorators;
using Template.Application.EventHandlers;
using Template.Application.Persistence;
using Template.Application.Queries;
using Template.Application.Services;
using Template.Domain.Entities;
using Template.Domain.Entities.Bonuses;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Entities.Customers;

namespace Template.Application
{
	internal static class ContainerBuilderExtensions
	{
		internal static void RegisterDecorators(this ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterDecorator<MessageBrokerMetricDecorator, IMessageBroker>();

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(EventHandlerMetricDecorator<>),
				serviceType: typeof(INotificationHandler<>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseEventHandler<,,>);
				});
			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(EventHandlingDecorator<>),
				serviceType: typeof(INotificationHandler<>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseEventHandler<,,>);
				});

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(CommandPersistenceMetricDecorator<,>),
				serviceType: typeof(ICommandPersistence<,>));

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(QueryRetrievalPersistenceMetricDecorator<,,>),
				serviceType: typeof(IQueryRetrievalPersistence<,,>));

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(QueryRetrievalPersistenceMetricDecorator<,>),
				serviceType: typeof(IQueryRetrievalPersistence<,>));

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(DistributedCacheWrapperMetricDecorator<,>),
				serviceType: typeof(IDistributedCacheWrapper<,>));

			containerBuilder.RegisterGenericDecorator(typeof(EntityRetrievalPersistenceCacheDecorator<,>), typeof(IEntityRetrievalPersistence<,>));

			containerBuilder.RegisterDecorator<AwardBonusMetricDecorator, IEntityModificationPersistence<long, long, Bonus, AwardModification>>();
			containerBuilder.RegisterDecorator<CreateBonusMetricDecorator, IEntityModificationPersistence<long, long, Bonus, CreateModification<long, long, Bonus>>>();
			containerBuilder.RegisterDecorator<CreateCampaignMetricDecorator, IEntityModificationPersistence<long, long, Campaign, CreateModification<long, long, Campaign>>>();
			containerBuilder.RegisterDecorator<CreateCustomerMetricDecorator, IEntityModificationPersistence<long, long, Customer, CreateModification<long, long, Customer>>>();
			containerBuilder.RegisterDecorator<DeleteCustomerMetricDecorator, IEntityModificationPersistence<long, long, Customer, DeleteModification<long, long, Customer>>>();
			containerBuilder.RegisterGenericDecorator(typeof(EntityModificationCacheDecorator<,,,>), typeof(IEntityModificationPersistence<,,,>));

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(QueryHandlerMetricDecorator<,>),
				serviceType: typeof(IRequestHandler<,>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseQueryHandler<,,>);
				});
			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(LogQueryDecorator<,>),
				serviceType: typeof(IRequestHandler<,>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseQueryHandler<,,>);
				});

			containerBuilder.RegisterDecorator<AddBonusMoneyMetricDecorator, IRequestHandler<AddBonusMoneyCommand, AddBonusMoneyCommandResponse>>();
			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(CommandHandlerMetricDecorator<,>),
				serviceType: typeof(IRequestHandler<,>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseCommandHandler<,,>);
				});
			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(LogCommandDecorator<,>),
				serviceType: typeof(IRequestHandler<,>),
				condition: _decoratorContext =>
				{
					return _decoratorContext.ImplementationType.BaseType.GetGenericTypeDefinition() == typeof(BaseCommandHandler<,,>);
				});
		}
	}
}