using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Customers;

namespace Template.Application.Commands.Customers
{
	internal class UpdateCustomerCommandHandler : BaseCommandHandler<long, UpdateCustomerCommand, UpdateCustomerCommandResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Customer> _entityRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Customer, UpdateModification<long, long, Customer>> _entityModificationPersistence;
		private readonly ILogger<UpdateCustomerCommandHandler> _logger;

		public UpdateCustomerCommandHandler(
			IEntityRetrievalPersistence<long, Customer> entityRetrievalPersistence,
			IEntityModificationPersistence<long, long, Customer, UpdateModification<long, long, Customer>> entityModificationPersistence,
			ILogger<UpdateCustomerCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<UpdateCustomerCommandResponse>> HandleInternal(UpdateCustomerCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var customer = await _entityRetrievalPersistence.Retrieve(command.CustomerId);
				if (customer == null)
					return new NotifiableResponse<UpdateCustomerCommandResponse>(UpdateCustomerCommandResponse.CreateError(command.Id, new Error[] { new Error($"Customer with id: [{command.CustomerId}] not found") }));
				if (customer.Email == command.Email && customer.DateOfBirth == command.DateOfBirth)
					return new NotifiableResponse<UpdateCustomerCommandResponse>(UpdateCustomerCommandResponse.CreateError(command.Id, new Error[] { new Error($"Customer with id: [{command.CustomerId}] not updated due to same values") }));

				var modification = customer.Update(command.Email, command.DateOfBirth);
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<UpdateCustomerCommandResponse>(UpdateCustomerCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Customer with id: [{customerId}] not updated", command.CustomerId);
				return new NotifiableResponse<UpdateCustomerCommandResponse>(UpdateCustomerCommandResponse.CreateError(command.Id, new Error[] { new Error($"Customer with id: [{command.CustomerId}] not updated") }));
			}
		}
	}
}