using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Services;
using Template.Domain.Entities.Bonuses;

namespace Template.Application.EventHandlers.Bonuses
{
	internal class BonusDeletedEventHandler : BaseEventHandler<long, long, BonusDeletedEvent>
	{
		private readonly IEntityRetrievalPersistence<long, Bonus> _entityRetrievalPersistence;
		private readonly IBonusDeletionNotifier _bonusDeletionNotifier;

		public BonusDeletedEventHandler(
			IEntityRetrievalPersistence<long, Bonus> entityRetrievalPersistence,
			IBonusDeletionNotifier bonusDeletionNotifier)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_bonusDeletionNotifier = bonusDeletionNotifier;
		}

		public override async Task Handle(BonusDeletedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				var bonus = await _entityRetrievalPersistence.Retrieve(@event.EntityId);
				if (bonus == null)
					throw new Exception($"Bonus with id: [{@event.EntityId}] not found");

				await _bonusDeletionNotifier.Notify(@event.EntityId);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}