using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities;
using Template.Domain.Entities.Campaigns;

namespace Template.Persistence.Commands.Campaigns
{
	internal class DeleteCampaignModificationPersistence : IEntityModificationPersistence<long, long, Campaign, DeleteModification<long, long, Campaign>>
	{
		private readonly ICampaignCommandStorage _campaignCommandStorage;

		public DeleteCampaignModificationPersistence(ICampaignCommandStorage campaignCommandStorage)
		{
			_campaignCommandStorage = campaignCommandStorage;
		}

		public async Task Persist(DeleteModification<long, long, Campaign> modification)
		{
			_campaignCommandStorage.Update(modification.Entity);
			await _campaignCommandStorage.SaveChangesAsync();
		}
	}
}