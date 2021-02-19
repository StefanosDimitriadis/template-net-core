using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Campaigns;

namespace Template.Application.EventHandlers.Campaigns
{
	internal class CampaignUpdatedEventHandler : BaseEventHandler<long, long, CampaignUpdatedEvent>
	{
		private readonly IEntityRetrievalPersistence<long, Campaign> _entityRetrievalPersistence;

		public CampaignUpdatedEventHandler(IEntityRetrievalPersistence<long, Campaign> entityRetrievalPersistence)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
		}

		public override async Task Handle(CampaignUpdatedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				var campaign = await _entityRetrievalPersistence.Retrieve(@event.EntityId);
				if (campaign == null)
					throw new Exception($"Campaign with id: [{@event.EntityId}] not found");
				if (campaign.CampaignSpecification == null)
					throw new Exception($"Campaign with id: [{campaign.Id}] does not have specification");
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}