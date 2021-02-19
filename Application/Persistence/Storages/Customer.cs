using System.Threading.Tasks;
using Template.Domain.Entities.Customers;

namespace Template.Application.Persistence.Storages
{
	public interface ICustomerCommandStorage
	{
		void Update(Customer customer);
		Task SaveChangesAsync();
		void Create(Customer customer);
		void Create(Customer[] customers);
	}

	public interface ICustomerQueryStorage
	{
		Task<Customer> Get(long id);
		Task<Customer[]> Get();
	}
}