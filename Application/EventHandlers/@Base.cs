using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.EventHandlers
{
	internal abstract class BaseEventHandler<TId, TEntityId, TEvent> : INotificationHandler<TEvent>
		where TEvent : BaseEvent<TId, TEntityId>
	{
		public abstract Task Handle(TEvent @event, CancellationToken cancellationToken);
	}
}