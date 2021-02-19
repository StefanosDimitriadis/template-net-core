using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Template.Application.Services;

namespace Template.Infrastructure.Services
{
	internal class MessageBroker : IMessageBroker
	{
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly ILogger<MessageBroker> _logger;

		public MessageBroker(
			IPublishEndpoint publishEndpoint,
			ILogger<MessageBroker> logger)
		{
			_publishEndpoint = publishEndpoint;
			_logger = logger;
		}

		public async Task Publish<TMessage>(TMessage message)
			where TMessage : BaseMessageBrokerMessage
		{
			await _publishEndpoint.Publish(message, _publishContext =>
			{
				Log(message, _publishContext);
			});
		}

		private void Log<TMessage>(TMessage message, PublishContext publishContext)
			where TMessage : BaseMessageBrokerMessage
		{
			var messagePayload = JsonConvert.SerializeObject(message);
			var conversationId = publishContext.ConversationId.HasValue ?
				publishContext.ConversationId.Value.ToString()
				: string.Empty;
			var correlationId = publishContext.CorrelationId.HasValue ?
				publishContext.ConversationId.Value.ToString()
				: string.Empty;
			var initiatorId = publishContext.InitiatorId.HasValue ?
				publishContext.InitiatorId.Value.ToString()
				: string.Empty;
			var messageId = publishContext.MessageId.HasValue ?
				publishContext.MessageId.Value.ToString()
				: string.Empty;
			var requestId = publishContext.RequestId.HasValue ?
				publishContext.RequestId.Value.ToString()
				: string.Empty;
			var scheduleMessageId = publishContext.ScheduledMessageId.HasValue ?
				publishContext.ScheduledMessageId.Value.ToString()
				: string.Empty;
			var sentTime = publishContext.SentTime.HasValue ?
				publishContext.SentTime.Value.ToString()
				: string.Empty;
			var logMessage = @$"Message with conversationId: [{conversationId}], correlationId: [{correlationId}],
								initiatorId: [{initiatorId}], messageId: [{messageId}], requestId: [{requestId}], scheduleMessageId: [{scheduleMessageId}],
								was sent at: [{sentTime}] and had payload: [{messagePayload}]";
			_logger.LogInformation(logMessage);
		}
	}
}