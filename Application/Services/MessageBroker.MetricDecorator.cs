using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Template.Application.Services;

namespace Template.Application.Services
{
	internal class MessageBrokerMetricDecorator : IMessageBroker
	{
		private readonly IMessageBroker _messageBroker;
		private readonly IMetrics _metrics;

		public MessageBrokerMetricDecorator(
			IMessageBroker messageBroker,
			IMetrics metrics)
		{
			_messageBroker = messageBroker;
			_metrics = metrics;
		}

		public async Task Publish<TMessage>(TMessage message) where TMessage : BaseMessageBrokerMessage
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				await _messageBroker.Publish(message);
				stopwatch.Stop();
				_metrics.MeasureTime(ApplicationMetricsRegistry.MessageBrokerPublishTimer, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}