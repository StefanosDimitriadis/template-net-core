using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Template.Application.RetrievalQueries;

namespace Template.Application.Persistence
{
	internal class QueryRetrievalPersistenceMetricDecorator<TId, TRequest, TResult> : IQueryRetrievalPersistence<TId, TRequest, TResult>
		where TRequest : BaseRequest<TId>
		where TResult : BaseResult<TId>
	{
		private readonly IQueryRetrievalPersistence<TId, TRequest, TResult> _queryRetrievalPersistence;
		private readonly IMetrics _metrics;

		public QueryRetrievalPersistenceMetricDecorator(
			IQueryRetrievalPersistence<TId, TRequest, TResult> queryRetrievalPersistence,
			IMetrics metrics)
		{
			_queryRetrievalPersistence = queryRetrievalPersistence;
			_metrics = metrics;
		}

		public async Task<TResult> Retrieve(TRequest request)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				var result = await _queryRetrievalPersistence.Retrieve(request);
				stopwatch.Stop();
				_metrics.MeasureTime(ApplicationMetricsRegistry.QueryRetrievalPersistenceTimer, stopwatch.ElapsedMilliseconds);
				return result;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	internal class QueryRetrievalPersistenceMetricDecorator<TId, TResult> : IQueryRetrievalPersistence<TId, TResult>
		where TResult : BaseResult<TId>
	{
		private readonly IQueryRetrievalPersistence<TId, TResult> _queryRetrievalPersistence;
		private readonly IMetrics _metrics;

		public QueryRetrievalPersistenceMetricDecorator(
			IQueryRetrievalPersistence<TId, TResult> queryRetrievalPersistence,
			IMetrics metrics)
		{
			_queryRetrievalPersistence = queryRetrievalPersistence;
			_metrics = metrics;
		}

		public async Task<TResult> Retrieve()
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				var result = await _queryRetrievalPersistence.Retrieve();
				stopwatch.Stop();
				_metrics.MeasureTime(ApplicationMetricsRegistry.QueryRetrievalPersistenceTimer, stopwatch.ElapsedMilliseconds);
				return result;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}