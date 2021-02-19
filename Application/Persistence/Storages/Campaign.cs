using System.Threading.Tasks;
using Template.Domain.Entities.Campaigns;

namespace Template.Application.Persistence.Storages
{
	public interface ICampaignCommandStorage
	{
		void Update(Campaign campaign);
		Task SaveChangesAsync();
		void Create(Campaign campaign);
		void Create(Campaign[] campaigns);
	}

	public interface ICampaignQueryStorage
	{
		Task<Campaign> Get(long id);
		Task<Campaign[]> Get();
	}
}