using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Customers;

namespace Template.Application.Commands.Customers
{
	internal class AddMoneyCommandHandler : BaseCommandHandler<long, AddMoneyCommand, AddMoneyCommandResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Customer> _entityRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Customer, AddMoneyModification> _entityModificationPersistence;
		private readonly ILogger<AddMoneyCommandHandler> _logger;

		public AddMoneyCommandHandler(
			IEntityRetrievalPersistence<long, Customer> entityRetrievalPersistence,
			IEntityModificationPersistence<long, long, Customer, AddMoneyModification> entityModificationPersistence,
			ILogger<AddMoneyCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<AddMoneyCommandResponse>> HandleInternal(AddMoneyCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var customer = await _entityRetrievalPersistence.Retrieve(command.CustomerId);
				if (customer == null)
					return new NotifiableResponse<AddMoneyCommandResponse>(AddMoneyCommandResponse.CreateError(command.Id, new Error[] { new Error($"Customer with id: [{command.CustomerId}] not found") }));

				var modification = customer.AddMoney(command.Money);
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<AddMoneyCommandResponse>(AddMoneyCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Money for customer with id: [{customerId}] not added", command.CustomerId);
				return new NotifiableResponse<AddMoneyCommandResponse>(AddMoneyCommandResponse.CreateError(command.Id, new Error[] { new Error($"Money for customer with id: [{command.CustomerId}] not added") }));
			}
		}
	}
}