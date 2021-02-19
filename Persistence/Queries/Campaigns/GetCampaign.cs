using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Campaigns;

namespace Template.Persistence.Queries.Campaigns
{
	internal class GetCampaignRetrievalPersistence : IEntityRetrievalPersistence<long, Campaign>
	{
		private readonly ICampaignQueryStorage _campaignQueryStorage;

		public GetCampaignRetrievalPersistence(ICampaignQueryStorage campaignQueryStorage)
		{
			_campaignQueryStorage = campaignQueryStorage;
		}

		public async Task<Campaign> Retrieve(long id)
		{
			return await _campaignQueryStorage.Get(id);
		}
	}
}