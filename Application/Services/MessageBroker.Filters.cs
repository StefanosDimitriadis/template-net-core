//using App.Metrics;
using GreenPipes;
using MassTransit;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Template.Application.Services
{
	public class MessageBrokerMetricFilter<TMessageBrokerMessage> : IFilter<ConsumeContext<TMessageBrokerMessage>>
		where TMessageBrokerMessage : BaseMessageBrokerMessage
	{
		//private readonly IMetrics _metrics;

		public MessageBrokerMetricFilter(/*IMetrics metrics*/)
		{
			//_metrics = metrics;
		}

		public void Probe(ProbeContext probeContext) { }

		public async Task Send(ConsumeContext<TMessageBrokerMessage> consumeContext, IPipe<ConsumeContext<TMessageBrokerMessage>> nextPipe)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				await nextPipe.Send(consumeContext);
				stopwatch.Stop();
				//_metrics.Measure.Timer.Time(ApplicationMetricsRegistry.MessageBrokerConsumerTimer, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}