using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Customers;

namespace Template.Application.EventHandlers.Customers
{
	internal class CustomerUpdatedEventHandler : BaseEventHandler<long, long, CustomerUpdatedEvent>
	{
		private readonly IEntityRetrievalPersistence<long, Customer> _entityRetrievalPersistence;

		public CustomerUpdatedEventHandler(IEntityRetrievalPersistence<long, Customer> entityRetrievalPersistence)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
		}

		public override async Task Handle(CustomerUpdatedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				var customer = await _entityRetrievalPersistence.Retrieve(@event.EntityId);
				if (customer == null)
					throw new Exception($"Customer with id: [{@event.EntityId}] not found");
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}