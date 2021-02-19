using System.Collections.Generic;
using Template.Application.RetrievalQueries;
using Template.Domain.Entities.Customers;
using Template.Domain.Services;

namespace Template.Persistence.RetrievalQueries
{
	public class ActiveCustomersRetrievalQueryResult : BaseResult<long>
	{
		public override long Id { get; protected set; }
		public IReadOnlyCollection<Customer> ActiveCustomers { get; }

		private ActiveCustomersRetrievalQueryResult(long id, IReadOnlyCollection<Customer> activeCustomers)
		{
			Id = id;
			ActiveCustomers = activeCustomers;
		}

		public static ActiveCustomersRetrievalQueryResult Create(IReadOnlyCollection<Customer> activeCustomers)
		{
			return new ActiveCustomersRetrievalQueryResult(IdGenerator.Generate(), activeCustomers);
		}
	}
}