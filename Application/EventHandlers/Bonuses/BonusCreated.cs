using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Services;
using Template.Domain.Entities.Bonuses;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Entities.Customers;

namespace Template.Application.EventHandlers.Bonuses
{
	internal class BonusCreatedEventHandler : BaseEventHandler<long, long, BonusCreatedEvent>
	{
		private readonly IEntityRetrievalPersistence<long, Bonus> _bonusRetrievalPersistence;
		private readonly IEntityRetrievalPersistence<long, Campaign> _campaignRetrievalPersistence;
		private readonly IEntityRetrievalPersistence<long, Customer> _customerRetrievalPersistence;
		private readonly ICustomerNotifier _customerNotifier;

		public BonusCreatedEventHandler(
			IEntityRetrievalPersistence<long, Bonus> bonusRetrievalPersistence,
			IEntityRetrievalPersistence<long, Campaign> campaignRetrievalPersistence,
			IEntityRetrievalPersistence<long, Customer> customerRetrievalPersistence,
			ICustomerNotifier customerNotifier)
		{
			_bonusRetrievalPersistence = bonusRetrievalPersistence;
			_campaignRetrievalPersistence = campaignRetrievalPersistence;
			_customerRetrievalPersistence = customerRetrievalPersistence;
			_customerNotifier = customerNotifier;
		}

		public override async Task Handle(BonusCreatedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				var bonus = await _bonusRetrievalPersistence.Retrieve(@event.EntityId);
				if (bonus == null)
					throw new Exception($"Bonus with id: [{@event.EntityId}] not found");

				var campaign = await _campaignRetrievalPersistence.Retrieve(bonus.CampaignId);
				if (campaign == null)
					throw new Exception($"Campaign with id: [{bonus.CampaignId}] not found");
				if (campaign.CampaignSpecification == null)
					throw new Exception($"Campaign with id: [{bonus.CampaignId}] does not have specification");

				var customer = await _customerRetrievalPersistence.Retrieve(bonus.CustomerId);
				if (customer == null)
					throw new Exception($"Customer with id: [{bonus.CustomerId}] not found");

				await _customerNotifier.NotifyForNewBonus(customer, campaign.CampaignSpecification);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}