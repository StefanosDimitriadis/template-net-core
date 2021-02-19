using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.Persistence
{
	public interface IEntityModificationPersistence<TEventId, TId, TEntity, TModification>
		where TEntity : BaseEntity<TId>
		where TModification : IModification<TEventId, TId, TEntity>
	{
		Task Persist(TModification modification);
	}
}