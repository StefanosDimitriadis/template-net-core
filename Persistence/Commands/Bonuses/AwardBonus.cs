using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Bonuses;

namespace Template.Persistence.Commands.Bonuses
{
	internal class AwardBonusModificationPersistence : IEntityModificationPersistence<long, long, Bonus, AwardModification>
	{
		private readonly IBonusCommandStorage _bonusCommandStorage;

		public AwardBonusModificationPersistence(IBonusCommandStorage bonusCommandStorage)
		{
			_bonusCommandStorage = bonusCommandStorage;
		}

		public async Task Persist(AwardModification modification)
		{
			_bonusCommandStorage.Update(modification.Entity);
			await _bonusCommandStorage.SaveChangesAsync();
		}
	}
}