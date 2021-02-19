using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities;
using Template.Domain.Entities.Bonuses;

namespace Template.Persistence.Commands.Bonuses
{
	internal class DeleteBonusModificationPersistence : IEntityModificationPersistence<long, long, Bonus, DeleteModification<long, long, Bonus>>
	{
		private readonly IBonusCommandStorage _bonusCommandStorage;

		public DeleteBonusModificationPersistence(IBonusCommandStorage bonusCommandStorage)
		{
			_bonusCommandStorage = bonusCommandStorage;
		}

		public async Task Persist(DeleteModification<long, long, Bonus> modification)
		{
			_bonusCommandStorage.Update(modification.Entity);
			await _bonusCommandStorage.SaveChangesAsync();
		}
	}
}