using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities;
using Template.Domain.Entities.Campaigns;

namespace Template.Persistence.Commands.Campaigns
{
	internal class CreateCampaignModificationPersistence : IEntityModificationPersistence<long, long, Campaign, CreateModification<long, long, Campaign>>
	{
		private readonly ICampaignCommandStorage _campaignCommandStorage;

		public CreateCampaignModificationPersistence(ICampaignCommandStorage campaignCommandStorage)
		{
			_campaignCommandStorage = campaignCommandStorage;
		}

		public async Task Persist(CreateModification<long, long, Campaign> modification)
		{
			_campaignCommandStorage.Create(modification.Entity);
			await _campaignCommandStorage.SaveChangesAsync();
		}
	}
}