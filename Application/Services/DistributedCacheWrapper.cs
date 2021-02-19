using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.Services
{
	public interface IDistributedCacheWrapper<TId, TEntity>
		where TEntity : BaseEntity<TId>
	{
		Task<TEntity> Get(TId id);
		Task Add(TEntity entity);
	}
}