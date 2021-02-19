using System;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Campaigns;

namespace Template.Application.Commands.Campaigns
{
	internal class CreateCampaignMetricDecorator : IEntityModificationPersistence<long, long, Campaign, CreateModification<long, long, Campaign>>
	{
		private readonly IEntityModificationPersistence<long, long, Campaign, CreateModification<long, long, Campaign>> _entityModificationPersistence;
		private readonly IMetrics _metrics;

		public CreateCampaignMetricDecorator(
			IEntityModificationPersistence<long, long, Campaign, CreateModification<long, long, Campaign>> entityModificationPersistence,
			IMetrics metrics)
		{
			_entityModificationPersistence = entityModificationPersistence;
			_metrics = metrics;
		}

		public async Task Persist(CreateModification<long, long, Campaign> modification)
		{
			try
			{
				await _entityModificationPersistence.Persist(modification);
				_metrics.IncreaseCounter(ApplicationMetricsRegistry.CreatedCampaignsCounter);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}