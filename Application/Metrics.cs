using App.Metrics.Counter;
using App.Metrics.Timer;

namespace Template.Application
{
	public interface IMetrics
	{
		void MeasureTime(TimerOptions timer, long elapsedMilliseconds);
		void IncreaseCounter(CounterOptions counter, long value = 1);
		void DecreaseCounter(CounterOptions counter, long value = 1);
	}

	internal class Metrics : IMetrics
	{
		private readonly App.Metrics.IMetrics _metrics;

		public Metrics(App.Metrics.IMetrics metrics)
		{
			_metrics = metrics;
		}

		public void DecreaseCounter(CounterOptions counter, long value = 1)
		{
			_metrics.Measure.Counter.Decrement(counter, value);
		}

		public void IncreaseCounter(CounterOptions counter, long value = 1)
		{
			_metrics.Measure.Counter.Increment(counter, value);
		}

		public void MeasureTime(TimerOptions timer, long elapsedMilliseconds)
		{
			_metrics.Measure.Timer.Time(timer, elapsedMilliseconds);
		}
	}
}