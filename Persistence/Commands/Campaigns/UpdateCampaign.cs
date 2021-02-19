using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities;
using Template.Domain.Entities.Campaigns;

namespace Template.Persistence.Commands.Campaigns
{
	internal class UpdateCampaignModificationPersistence : IEntityModificationPersistence<long, long, Campaign, UpdateModification<long, long, Campaign>>
	{
		private readonly ICampaignCommandStorage _campaignCommandStorage;

		public UpdateCampaignModificationPersistence(ICampaignCommandStorage campaignCommandStorage)
		{
			_campaignCommandStorage = campaignCommandStorage;
		}

		public async Task Persist(UpdateModification<long, long, Campaign> modification)
		{
			_campaignCommandStorage.Update(modification.Entity);
			await _campaignCommandStorage.SaveChangesAsync();
		}
	}
}