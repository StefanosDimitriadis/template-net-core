using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.Persistence.Storages
{
	public interface IEventCommandStorage<TId, TEntityId>
	{
		Task Create(BaseEvent<TId, TEntityId> @event);
		Task Update(BaseEvent<TId, TEntityId> @event);
	}
}