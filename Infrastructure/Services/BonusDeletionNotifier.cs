using System.Threading.Tasks;
using Template.Application.Services;

namespace Template.Infrastructure.Services
{
	internal class BonusDeletionNotifier : IBonusDeletionNotifier
	{
		private readonly IMessageBroker _messageBroker;

		public BonusDeletionNotifier(IMessageBroker messageBroker)
		{
			_messageBroker = messageBroker;
		}

		public async Task Notify(long bonusId)
		{
			await _messageBroker.Publish(new BonusDeletionMessage(bonusId));
		}
	}
}