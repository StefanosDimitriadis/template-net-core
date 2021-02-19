using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Template.Application.Services
{
	public abstract class BaseMessageBrokerConsumer<TMessageBrokerMessage> : IConsumer<TMessageBrokerMessage>
		where TMessageBrokerMessage : BaseMessageBrokerMessage
	{
		public abstract Task Consume(ConsumeContext<TMessageBrokerMessage> context);
	}

	public class BonusDeletionMessageBrokerConsumer : BaseMessageBrokerConsumer<BonusDeletionMessage>
	{
		private readonly ILogger<BonusDeletionMessage> _logger;

		public BonusDeletionMessageBrokerConsumer(ILogger<BonusDeletionMessage> logger)
		{
			_logger = logger;
		}

		public override async Task Consume(ConsumeContext<BonusDeletionMessage> consumeContext)
		{
			try
			{
				_logger.LogInformation("Bonus deletion message with bonusId: [{bonusId}] consumed", consumeContext.Message.BonusId);
				await Task.CompletedTask;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Bonus deletion message with bonusId: [{bonusId}]) consumed with error", consumeContext.Message.BonusId);
			}
		}
	}
}