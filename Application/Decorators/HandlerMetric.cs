using MediatR;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Application.Decorators
{
	internal class QueryHandlerMetricDecorator<TQuery, TQueryResponse> : IRequestHandler<TQuery, TQueryResponse>
		where TQuery : IRequest<TQueryResponse>
	{
		private readonly IRequestHandler<TQuery, TQueryResponse> _queryHandler;
		private readonly IMetrics _metrics;

		public QueryHandlerMetricDecorator(
			IRequestHandler<TQuery, TQueryResponse> queryHandler,
			IMetrics metrics)
		{
			_queryHandler = queryHandler;
			_metrics = metrics;
		}

		public async Task<TQueryResponse> Handle(TQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				var queryResponse = await _queryHandler.Handle(query, cancellationToken);
				stopwatch.Stop();
				_metrics.MeasureTime(ApplicationMetricsRegistry.QueryHandlerTimer, stopwatch.ElapsedMilliseconds);
				return queryResponse;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	internal class CommandHandlerMetricDecorator<TCommand, TCommandResponse> : IRequestHandler<TCommand, TCommandResponse>
		where TCommand : IRequest<TCommandResponse>
	{
		private readonly IRequestHandler<TCommand, TCommandResponse> _commandHandler;
		private readonly IMetrics _metrics;

		public CommandHandlerMetricDecorator(
			IRequestHandler<TCommand, TCommandResponse> commandHandler,
			IMetrics metrics)
		{
			_commandHandler = commandHandler;
			_metrics = metrics;
		}

		public async Task<TCommandResponse> Handle(TCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				var commandResponse = await _commandHandler.Handle(command, cancellationToken);
				stopwatch.Stop();
				_metrics.MeasureTime(ApplicationMetricsRegistry.CommandHandlerTimer, stopwatch.ElapsedMilliseconds);
				return commandResponse;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	internal class EventHandlerMetricDecorator<TNotification> : INotificationHandler<TNotification>
		where TNotification : INotification
	{
		private readonly INotificationHandler<TNotification> _notificationHandler;
		private readonly IMetrics _metrics;

		public EventHandlerMetricDecorator(
			INotificationHandler<TNotification> notificationHandler,
			IMetrics metrics)
		{
			_notificationHandler = notificationHandler;
			_metrics = metrics;
		}

		public async Task Handle(TNotification notification, CancellationToken cancellationToken)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				await _notificationHandler.Handle(notification, cancellationToken);
				stopwatch.Stop();
				_metrics.MeasureTime(ApplicationMetricsRegistry.NotificationHandlerTimer, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}