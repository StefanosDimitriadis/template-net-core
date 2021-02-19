using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.Persistence
{
	public interface IEntityRetrievalPersistence<TId, TEntity>
		where TEntity : BaseEntity<TId>
	{
		Task<TEntity> Retrieve(TId id);
	}
}