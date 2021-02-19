using System.Threading.Tasks;
using Template.Application.PersistenceCommands;

namespace Template.Application.Persistence
{
	public interface ICommandPersistence<TId, TPersistCommand>
		where TPersistCommand : BasePersistenceCommand<TId>
	{
		Task Persist(TPersistCommand persistCommand);
	}
}