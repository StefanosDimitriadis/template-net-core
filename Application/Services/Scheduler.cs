using System.Threading.Tasks;

namespace Template.Application.Services
{
	public interface IScheduler
	{
		Task CreateNotifyPlayersOnFirstOfMonth();
		Task RemoveAll();
	}
}