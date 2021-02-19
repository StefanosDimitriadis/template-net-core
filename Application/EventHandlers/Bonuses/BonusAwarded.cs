using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Commands.Customers;
using Template.Application.Persistence;
using Template.Domain.Entities.Bonuses;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Entities.Customers;

namespace Template.Application.EventHandlers.Bonuses
{
	internal class BonusAwardedEventHandler : BaseEventHandler<long, long, BonusAwardedEvent>
	{
		private readonly IEntityRetrievalPersistence<long, Bonus> _bonusRetrievalPersistence;
		private readonly IEntityRetrievalPersistence<long, Campaign> _campaignRetrievalPersistence;
		private readonly IEntityRetrievalPersistence<long, Customer> _customerRetrievalPersistence;
		private readonly IMediator _mediator;

		public BonusAwardedEventHandler(
			IEntityRetrievalPersistence<long, Bonus> bonusRetrievalPersistence,
			IEntityRetrievalPersistence<long, Campaign> campaignRetrievalPersistence,
			IEntityRetrievalPersistence<long, Customer> customerRetrievalPersistence,
			IMediator mediator)
		{
			_bonusRetrievalPersistence = bonusRetrievalPersistence;
			_campaignRetrievalPersistence = campaignRetrievalPersistence;
			_customerRetrievalPersistence = customerRetrievalPersistence;
			_mediator = mediator;
		}

		public override async Task Handle(BonusAwardedEvent @event, CancellationToken cancellationToken)
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

				var addBonusMoneyCommand = AddBonusMoneyCommand.Create(customer.Id, campaign.CampaignSpecification.BonusMoney);
				await _mediator.Send(addBonusMoneyCommand, cancellationToken);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}