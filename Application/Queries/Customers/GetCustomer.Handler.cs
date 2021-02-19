using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Customers;

namespace Template.Application.Queries.Customers
{
	internal class GetCustomerQueryHandler : BaseQueryHandler<long, GetCustomerQuery, GetCustomerQueryResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Customer> _entityRetrievalPersistence;
		private readonly ILogger<GetCustomerQueryHandler> _logger;

		public GetCustomerQueryHandler(
			IEntityRetrievalPersistence<long, Customer> entityRetrievalPersistence,
			ILogger<GetCustomerQueryHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<GetCustomerQueryResponse>> HandleInternal(GetCustomerQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var customer = await _entityRetrievalPersistence.Retrieve(query.CustomerId);
				if (customer == null)
					return GetErrorResponse(query.Id, query.CustomerId);

				return new NotifiableResponse<GetCustomerQueryResponse>(GetCustomerQueryResponse.Create(query.Id, customer));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Customer with id: [{customerId}] not found", query.CustomerId);
				return GetErrorResponse(query.Id, query.CustomerId);
			}
		}

		private NotifiableResponse<GetCustomerQueryResponse> GetErrorResponse(long queryId, long customerId)
		{
			return new NotifiableResponse<GetCustomerQueryResponse>(GetCustomerQueryResponse.CreateError(queryId, new Error[] { new Error($"Customer with id: [{customerId}] not found") }));
		}
	}
}