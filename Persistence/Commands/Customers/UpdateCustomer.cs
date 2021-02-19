using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities;
using Template.Domain.Entities.Customers;

namespace Template.Persistence.Commands.Customers
{
	internal class UpdateCustomerModificationPersistence : IEntityModificationPersistence<long, long, Customer, UpdateModification<long, long, Customer>>
	{
		private readonly ICustomerCommandStorage _customerCommandStorage;

		public UpdateCustomerModificationPersistence(ICustomerCommandStorage customerCommandStorage)
		{
			_customerCommandStorage = customerCommandStorage;
		}

		public async Task Persist(UpdateModification<long, long, Customer> modification)
		{
			_customerCommandStorage.Update(modification.Entity);
			await _customerCommandStorage.SaveChangesAsync();
		}
	}
}