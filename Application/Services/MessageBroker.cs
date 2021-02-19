using System.Threading.Tasks;

namespace Template.Application.Services
{
	public interface IMessageBroker
	{
		Task Publish<TMessage>(TMessage message)
			where TMessage : BaseMessageBrokerMessage;
	}
}