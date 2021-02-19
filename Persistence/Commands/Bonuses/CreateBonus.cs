using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities;
using Template.Domain.Entities.Bonuses;

namespace Template.Persistence.Commands.Bonuses
{
	internal class CreateBonusModificationPersistence : IEntityModificationPersistence<long, long, Bonus, CreateModification<long, long, Bonus>>
	{
		private readonly IBonusCommandStorage _bonusCommandStorage;

		public CreateBonusModificationPersistence(IBonusCommandStorage bonusCommandStorage)
		{
			_bonusCommandStorage = bonusCommandStorage;
		}

		public async Task Persist(CreateModification<long, long, Bonus> modification)
		{
			_bonusCommandStorage.Create(modification.Entity);
			await _bonusCommandStorage.SaveChangesAsync();
		}
	}
}