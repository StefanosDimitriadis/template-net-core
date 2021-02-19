using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities;
using Template.Domain.Entities.Customers;

namespace Template.Persistence.Commands.Customers
{
	internal class DeleteCustomerModificationPersistence : IEntityModificationPersistence<long, long, Customer, DeleteModification<long, long, Customer>>
	{
		private readonly ICustomerCommandStorage _customerCommandStorage;

		public DeleteCustomerModificationPersistence(ICustomerCommandStorage customerCommandStorage)
		{
			_customerCommandStorage = customerCommandStorage;
		}

		public async Task Persist(DeleteModification<long, long, Customer> modification)
		{
			_customerCommandStorage.Update(modification.Entity);
			await _customerCommandStorage.SaveChangesAsync();
		}
	}
}