using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Services;
using Template.Domain.Entities.Customers;

namespace Template.Application.EventHandlers.Customers
{
	internal class CustomerCreatedEventHandler : BaseEventHandler<long, long, CustomerCreatedEvent>
	{
		private readonly IEntityRetrievalPersistence<long, Customer> _entityRetrievalPersistence;
		private readonly ICustomerNotifier _customerNotifier;

		public CustomerCreatedEventHandler(
			IEntityRetrievalPersistence<long, Customer> entityRetrievalPersistence,
			ICustomerNotifier customerNotifier)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_customerNotifier = customerNotifier;
		}

		public override async Task Handle(CustomerCreatedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				var customer = await _entityRetrievalPersistence.Retrieve(@event.EntityId);
				if (customer == null)
					throw new Exception($"Customer with id: [{@event.EntityId}] not found");

				await _customerNotifier.NotifyForRegistration(customer);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}