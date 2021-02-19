using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Bonuses;

namespace Template.Persistence.Queries.Bonuses
{
	internal class GetBonusRetrievalPersistence : IEntityRetrievalPersistence<long, Bonus>
	{
		private readonly IBonusQueryStorage _bonusQueryStorage;

		public GetBonusRetrievalPersistence(IBonusQueryStorage bonusQueryStorage)
		{
			_bonusQueryStorage = bonusQueryStorage;
		}

		public async Task<Bonus> Retrieve(long id)
		{
			return await _bonusQueryStorage.Get(id);
		}
	}
}