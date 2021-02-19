using System.Threading.Tasks;

namespace Template.Application.Services
{
	public interface IBonusDeletionNotifier
	{
		Task Notify(long bonusId);
	}
}