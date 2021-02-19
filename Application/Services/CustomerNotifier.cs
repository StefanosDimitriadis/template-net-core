using System.Threading.Tasks;
using Template.Domain.Entities.Customers;
using Template.Domain.Values;

namespace Template.Application.Services
{
	public interface ICustomerNotifier
	{
		Task NotifyForRegistration(Customer customer);
		Task NotifyForNewBonus(Customer customer, CampaignSpecification campaignSpecification);
		Task NotifyForAward(Customer customer, decimal moneyToAdd);
		Task NotifyOnFirstOfMonth(Customer customer);
	}
}