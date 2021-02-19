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
	internal class DeleteCustomerCommandHandler : BaseCommandHandler<long, DeleteCustomerCommand, DeleteCustomerCommandResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Customer> _entityRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Customer, DeleteModification<long, long, Customer>> _entityModificationPersistence;
		private readonly ILogger<DeleteCustomerCommandHandler> _logger;

		public DeleteCustomerCommandHandler(
			IEntityRetrievalPersistence<long, Customer> entityRetrievalPersistence,
			IEntityModificationPersistence<long, long, Customer, DeleteModification<long, long, Customer>> entityModificationPersistence,
			ILogger<DeleteCustomerCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<DeleteCustomerCommandResponse>> HandleInternal(DeleteCustomerCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var customer = await _entityRetrievalPersistence.Retrieve(command.CustomerId);
				if (customer == null)
					return new NotifiableResponse<DeleteCustomerCommandResponse>(DeleteCustomerCommandResponse.CreateError(command.Id, new Error[] { new Error($"Customer with id: [{command.CustomerId}] not found") }));
				if (customer.IsDeleted)
					return new NotifiableResponse<DeleteCustomerCommandResponse>(DeleteCustomerCommandResponse.CreateError(command.Id, new Error[] { new Error($"Customer with id: [{command.CustomerId}] is already deleted") }));

				var modification = customer.Delete();
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<DeleteCustomerCommandResponse>(DeleteCustomerCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Customer with id: [{customerId}] not deleted", command.CustomerId);
				return new NotifiableResponse<DeleteCustomerCommandResponse>(DeleteCustomerCommandResponse.CreateError(command.Id, new Error[] { new Error($"Customer with id: [{command.CustomerId}] not deleted") }));
			}
		}
	}
}