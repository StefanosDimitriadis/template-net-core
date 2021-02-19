using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Customers;

namespace Template.Persistence.Commands.Customers
{
	internal class AddMoneyModificationPersistence : IEntityModificationPersistence<long, long, Customer, AddMoneyModification>
	{
		private readonly ICustomerCommandStorage _customerCommandStorage;

		public AddMoneyModificationPersistence(ICustomerCommandStorage customerCommandStorage)
		{
			_customerCommandStorage = customerCommandStorage;
		}

		public async Task Persist(AddMoneyModification modification)
		{
			_customerCommandStorage.Update(modification.Entity);
			await _customerCommandStorage.SaveChangesAsync();
		}
	}
}