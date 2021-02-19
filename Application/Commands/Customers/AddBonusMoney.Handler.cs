using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Services;
using Template.Domain.Entities.Customers;

namespace Template.Application.Commands.Customers
{
	internal class AddBonusMoneyCommandHandler : BaseCommandHandler<long, AddBonusMoneyCommand, AddBonusMoneyCommandResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Customer> _entityRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Customer, AddMoneyModification> _entityModificationPersistence;
		private readonly ICustomerNotifier _customerNotifier;
		private readonly ILogger<AddBonusMoneyCommandHandler> _logger;

		public AddBonusMoneyCommandHandler(
			IEntityRetrievalPersistence<long, Customer> entityRetrievalPersistence,
			IEntityModificationPersistence<long, long, Customer, AddMoneyModification> entityModificationPersistence,
			ICustomerNotifier customerNotifier,
			ILogger<AddBonusMoneyCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_customerNotifier = customerNotifier;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<AddBonusMoneyCommandResponse>> HandleInternal(AddBonusMoneyCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var customer = await _entityRetrievalPersistence.Retrieve(command.CustomerId);
				if (customer == null)
					return new NotifiableResponse<AddBonusMoneyCommandResponse>(AddBonusMoneyCommandResponse.CreateError(command.Id, new Error[] { new Error($"Customer with id: [{command.CustomerId}] not found") }));

				var modification = customer.AddMoney(command.Money);
				await _entityModificationPersistence.Persist(modification);
				await _customerNotifier.NotifyForAward(customer, command.Money);
				return new NotifiableResponse<AddBonusMoneyCommandResponse>(AddBonusMoneyCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Money for customer with id: [{customerId}] not added", command.CustomerId);
				return new NotifiableResponse<AddBonusMoneyCommandResponse>(AddBonusMoneyCommandResponse.CreateError(command.Id, new Error[] { new Error($"Money for customer with id: [{command.CustomerId}] not added") }));
			}
		}
	}
}