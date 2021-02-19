using System.Collections.Generic;
using Template.Domain.Entities.Customers;
using Template.Domain.Services;

namespace Template.Application.Queries.Customers
{
	public class GetCustomerQuery : BaseQuery<long, GetCustomerQueryResponse>
	{
		public override long Id { get; protected set; }
		public long CustomerId { get; }

		private GetCustomerQuery(long id, long customerId)
		{
			Id = id;
			CustomerId = customerId;
		}

		public static GetCustomerQuery Create(long customerId)
		{
			return new GetCustomerQuery(IdGenerator.Generate(), customerId);
		}
	}

	public class GetCustomerQueryResponse : BaseQueryResponse<long>
	{
		public override long Id { get; protected set; }
		public Customer Customer { get; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private GetCustomerQueryResponse(long id, Customer customer)
			: base(id)
		{
			Customer = customer;
		}

		private GetCustomerQueryResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static GetCustomerQueryResponse Create(long id, Customer customer)
		{
			return new GetCustomerQueryResponse(id, customer);
		}

		public static GetCustomerQueryResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new GetCustomerQueryResponse(id, errors);
		}
	}
}