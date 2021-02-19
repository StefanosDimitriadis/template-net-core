using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Customers;
using Template.Persistence.RetrievalQueries;

namespace Template.Application.Commands.Customers
{
	internal class CreateCustomerCommandHandler : BaseCommandHandler<long, CreateCustomerCommand, CreateCustomerCommandResponse>
	{
		private readonly IQueryRetrievalPersistence<long, ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest, ExistingCustomersWithSameNameAndEmailRetrievalQueryResult> _queryRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Customer, CreateModification<long, long, Customer>> _entityModificationPersistence;
		private readonly ILogger<CreateCustomerCommandHandler> _logger;

		public CreateCustomerCommandHandler(
			IQueryRetrievalPersistence<long, ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest, ExistingCustomersWithSameNameAndEmailRetrievalQueryResult> queryRetrievalPersistence,
			IEntityModificationPersistence<long, long, Customer, CreateModification<long, long, Customer>> entityModificationPersistence,
			ILogger<CreateCustomerCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_queryRetrievalPersistence = queryRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<CreateCustomerCommandResponse>> HandleInternal(CreateCustomerCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var retrievalQueryResult = await _queryRetrievalPersistence.Retrieve(ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest.Create(command.Name, command.Email));
				if (retrievalQueryResult.ExistingCustomersWithSameNameAndEmail)
					return new NotifiableResponse<CreateCustomerCommandResponse>(CreateCustomerCommandResponse.CreateError(command.Id, new Error[] { new Error("Customer already exists") }));

				var modification = Customer.Create(command.Name, command.Email, command.DateOfBirth);
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<CreateCustomerCommandResponse>(CreateCustomerCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Customer not created");
				return new NotifiableResponse<CreateCustomerCommandResponse>(CreateCustomerCommandResponse.CreateError(command.Id, new Error[] { new Error("Customer not created") }));
			}
		}
	}
}