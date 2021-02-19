using App.Metrics;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;

namespace Template.Application.Decorators
{
	internal class EntityModificationPersistenceMetricDecorator<TEventId, TId, TEntity, TModification> : IEntityModificationPersistence<TEventId, TId, TEntity, TModification>
		where TEntity : BaseEntity<TId>
		where TModification : IModification<TEventId, TId, TEntity>
	{
		private readonly IEntityModificationPersistence<TEventId, TId, TEntity, TModification> _entityModificationPersistence;
		private readonly IMetrics _metrics;

		public EntityModificationPersistenceMetricDecorator(
			IEntityModificationPersistence<TEventId, TId, TEntity, TModification> entityModificationPersistence,
			IMetrics metrics)
		{
			_entityModificationPersistence = entityModificationPersistence;
			_metrics = metrics;
		}

		public async Task Persist(TModification modification)
		{
			try
			{
				var stopwatch = Stopwatch.StartNew();
				await _entityModificationPersistence.Persist(modification);
				stopwatch.Stop();
				_metrics.Measure.Timer.Time(ApplicationMetricsRegistry.EntityModificationPersistenceTimer, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}