using System;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Bonuses;

namespace Template.Application.Commands.Bonuses
{
	internal class AwardBonusMetricDecorator : IEntityModificationPersistence<long, long, Bonus, AwardModification>
	{
		private readonly IEntityModificationPersistence<long, long, Bonus, AwardModification> _entityModificationPersistence;
		private readonly IMetrics _metrics;

		public AwardBonusMetricDecorator(
			IEntityModificationPersistence<long, long, Bonus, AwardModification> entityModificationPersistence,
			IMetrics metrics)
		{
			_entityModificationPersistence = entityModificationPersistence;
			_metrics = metrics;
		}

		public async Task Persist(AwardModification modification)
		{
			try
			{
				await _entityModificationPersistence.Persist(modification);
				_metrics.IncreaseCounter(ApplicationMetricsRegistry.AwardedBonusesCounter);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}