using App.Metrics;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Template.Application.Services;
using Template.Domain.Entities;

namespace Template.Application.Decorators
{
	internal class DistributedCacheWrapperMetricDecorator<TId, TEntity> : IDistributedCacheWrapper<TId, TEntity>
		where TEntity : BaseEntity<TId>
	{
		private readonly IDistributedCacheWrapper<TId, TEntity> _distributedCacheWrapper;
		private readonly IMetrics _metrics;

		public DistributedCacheWrapperMetricDecorator(
			IDistributedCacheWrapper<TId, TEntity> distributedCacheWrapper,
			IMetrics metrics)
		{
			_distributedCacheWrapper = distributedCacheWrapper;
			_metrics = metrics;
		}

		public async Task Add(TEntity entity)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				await _distributedCacheWrapper.Add(entity);
				stopwatch.Stop();
				_metrics.Measure.Timer.Time(ApplicationMetricsRegistry.DistributedCacheWrapperAddTimer, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<TEntity> Get(TId id)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				var entity = await _distributedCacheWrapper.Get(id);
				stopwatch.Stop();
				_metrics.Measure.Timer.Time(ApplicationMetricsRegistry.DistributedCacheWrapperGetTimer, stopwatch.ElapsedMilliseconds);
				return entity;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}