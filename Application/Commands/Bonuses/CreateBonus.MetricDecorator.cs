using System;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Bonuses;

namespace Template.Application.Commands.Bonuses
{
	internal class CreateBonusMetricDecorator : IEntityModificationPersistence<long, long, Bonus, CreateModification<long, long, Bonus>>
	{
		private readonly IEntityModificationPersistence<long, long, Bonus, CreateModification<long, long, Bonus>> _entityModificationPersistence;
		private readonly IMetrics _metrics;

		public CreateBonusMetricDecorator(
			IEntityModificationPersistence<long, long, Bonus, CreateModification<long, long, Bonus>> entityModificationPersistence,
			IMetrics metrics)
		{
			_entityModificationPersistence = entityModificationPersistence;
			_metrics = metrics;
		}

		public async Task Persist(CreateModification<long, long, Bonus> modification)
		{
			try
			{
				await _entityModificationPersistence.Persist(modification);
				_metrics.IncreaseCounter(ApplicationMetricsRegistry.CreatedBonusesCounter);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}