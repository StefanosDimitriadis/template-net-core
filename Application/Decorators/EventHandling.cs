using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Application.PersistenceCommands.Events;
using Template.Domain.Entities;

namespace Template.Application.Decorators
{
	internal class EventHandlingDecorator<TEvent> : INotificationHandler<TEvent>
		where TEvent : INotification
	{
		private readonly INotificationHandler<TEvent> _eventHandler;
		private readonly ICommandPersistence<long, CreateEventPersistenceCommand<BaseEvent<long, long>, long, long>> _createEventPersistenceCommand;
		private readonly ICommandPersistence<long, UpdateEventPersistenceCommand<BaseEvent<long, long>, long, long>> _updateEventPersistenceCommand;
		private readonly ILogger<EventHandlingDecorator<TEvent>> _logger;

		public EventHandlingDecorator(
			INotificationHandler<TEvent> eventHandler,
			ICommandPersistence<long, CreateEventPersistenceCommand<BaseEvent<long, long>, long, long>> createEventPersistenceCommand,
			ICommandPersistence<long, UpdateEventPersistenceCommand<BaseEvent<long, long>, long, long>> updateEventPersistenceCommand,
			ILogger<EventHandlingDecorator<TEvent>> logger)
		{
			_eventHandler = eventHandler;
			_createEventPersistenceCommand = createEventPersistenceCommand;
			_updateEventPersistenceCommand = updateEventPersistenceCommand;
			_logger = logger;
		}

		public async Task Handle(TEvent @event, CancellationToken cancellationToken)
		{
			var baseEvent = @event as BaseEvent<long, long>;
			try
			{
				await _createEventPersistenceCommand.Persist(new CreateEventPersistenceCommand<BaseEvent<long, long>, long, long>(baseEvent.Id, baseEvent));
				_logger.LogInformation("Event with id: [{eventId}] was created at: [{eventCreationDate}]", baseEvent.Id, baseEvent.CreatedAt);
				await _eventHandler.Handle(@event, cancellationToken);
				baseEvent.SetHandled();
				await _updateEventPersistenceCommand.Persist(new UpdateEventPersistenceCommand<BaseEvent<long, long>, long, long>(baseEvent.Id, @event as BaseEvent<long, long>));
				_logger.LogInformation("Event with id: [{eventId}] was handled at: [{eventHandleDate}]", baseEvent.Id, baseEvent.HandledAt);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Event with id: [{eventId}] was not handled", baseEvent.Id);
			}
		}
	}
}