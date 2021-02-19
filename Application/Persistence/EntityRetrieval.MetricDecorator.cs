using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.Persistence
{
	internal class EntityRetrievalPersistenceMetricDecorator<TId, TEntity> : IEntityRetrievalPersistence<TId, TEntity>
		where TEntity : BaseEntity<TId>
	{
		private readonly IEntityRetrievalPersistence<TId, TEntity> _entityRetrievalPersistence;
		private readonly IMetrics _metrics;

		public EntityRetrievalPersistenceMetricDecorator(
			IEntityRetrievalPersistence<TId, TEntity> entityRetrievalPersistence,
			IMetrics metrics)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_metrics = metrics;
		}

		public async Task<TEntity> Retrieve(TId id)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				var entity = await _entityRetrievalPersistence.Retrieve(id);
				stopwatch.Stop();
				_metrics.MeasureTime(ApplicationMetricsRegistry.EntityRetrievalPersistenceTimer, stopwatch.ElapsedMilliseconds);
				return entity;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}