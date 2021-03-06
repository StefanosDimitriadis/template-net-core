﻿using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.Application.Commands;
using Template.Application.Commands.Bonuses;
using Template.Application.Commands.Campaigns;
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
	public static class MetricsExtensions
	{
		public static class Endpoints
		{
			public const string Metrics = "/metrics";
			public const string MetricsText = "/metrics-text";
		}

		public static void ConfigureMetricsHosting(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseMetrics(_metricsWebHostOptions => _metricsWebHostOptions.EndpointOptions = _metricEndpointsOptions =>
			{
				_metricEndpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
				_metricEndpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
				_metricEndpointsOptions.EnvironmentInfoEndpointEnabled = false;
			});
			hostBuilder.ConfigureAppMetricsHostingConfiguration(_metricsEndpointsHostingOptions =>
			{
				_metricsEndpointsHostingOptions.MetricsEndpoint = Endpoints.Metrics;
				_metricsEndpointsHostingOptions.MetricsTextEndpoint = Endpoints.MetricsText;
			});
		}

		public static void AddMetricServices(this IServiceCollection services)
		{
			services.AddMetrics();
			services.AddMetricsEndpoints();
			services.AddMetricsReportingHostedService((sender, eventArgs) =>
			{
				var logger = LogManager.GetCurrentClassLogger(projectName: nameof(Application), className: nameof(MetricsExtensions));
				logger.Error(eventArgs.Exception);
			});
			services.AddMetricsTrackingMiddleware();
		}

		public static void UseMetricsMiddlewares(this IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.UseMetricsEndpoint();
			applicationBuilder.UseMetricsTextEndpoint();
			applicationBuilder.UseMetricsActiveRequestMiddleware();
			applicationBuilder.UseMetricsApdexTrackingMiddleware();
			applicationBuilder.UseMetricsErrorTrackingMiddleware();
			applicationBuilder.UseMetricsOAuth2TrackingMiddleware();
			applicationBuilder.UseMetricsPostAndPutSizeTrackingMiddleware();
			applicationBuilder.UseMetricsRequestTrackingMiddleware();
		}

		internal static void RegisterMetricsDecorators(this ContainerBuilder containerBuilder)
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

			containerBuilder.RegisterDecorator<AwardBonusMetricDecorator, IEntityModificationPersistence<long, long, Bonus, AwardModification>>();
			containerBuilder.RegisterDecorator<CreateBonusMetricDecorator, IEntityModificationPersistence<long, long, Bonus, CreateModification<long, long, Bonus>>>();
			containerBuilder.RegisterDecorator<CreateCampaignMetricDecorator, IEntityModificationPersistence<long, long, Campaign, CreateModification<long, long, Campaign>>>();
			containerBuilder.RegisterDecorator<CreateCustomerMetricDecorator, IEntityModificationPersistence<long, long, Customer, CreateModification<long, long, Customer>>>();
			containerBuilder.RegisterDecorator<DeleteCustomerMetricDecorator, IEntityModificationPersistence<long, long, Customer, DeleteModification<long, long, Customer>>>();

			containerBuilder.RegisterGenericDecorator(
				decoratorType: typeof(QueryHandlerMetricDecorator<,>),
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
		}
	}
}