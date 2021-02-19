using System.Threading.Tasks;
using Template.Domain.Entities.Bonuses;

namespace Template.Application.Persistence.Storages
{
	public interface IBonusCommandStorage
	{
		void Update(Bonus bonus);
		Task SaveChangesAsync();
		void Create(Bonus bonus);
	}

	public interface IBonusQueryStorage
	{
		Task<Bonus> Get(long id);
		Task<Bonus[]> Get();
	}
}