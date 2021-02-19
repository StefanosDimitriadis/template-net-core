using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Customers;

namespace Template.Persistence.Queries.Customers
{
	internal class GetCustomerRetrievalPersistence : IEntityRetrievalPersistence<long, Customer>
	{
		private readonly ICustomerQueryStorage _customerQueryStorage;

		public GetCustomerRetrievalPersistence(ICustomerQueryStorage customerQueryStorage)
		{
			_customerQueryStorage = customerQueryStorage;
		}

		public async Task<Customer> Retrieve(long id)
		{
			return await _customerQueryStorage.Get(id);
		}
	}
}