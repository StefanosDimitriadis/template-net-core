using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Application.Commands.Customers
{
	internal class AddBonusMoneyMetricDecorator : IRequestHandler<AddBonusMoneyCommand, AddBonusMoneyCommandResponse>
	{
		private readonly IRequestHandler<AddBonusMoneyCommand, AddBonusMoneyCommandResponse> _requestHandler;
		private readonly IMetrics _metrics;

		public AddBonusMoneyMetricDecorator(
			IRequestHandler<AddBonusMoneyCommand, AddBonusMoneyCommandResponse> requestHandler,
			IMetrics metrics)
		{
			_requestHandler = requestHandler;
			_metrics = metrics;
		}

		public async Task<AddBonusMoneyCommandResponse> Handle(AddBonusMoneyCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var commandResponse = await _requestHandler.Handle(command, cancellationToken);
				if (commandResponse.Errors != null)
					_metrics.IncreaseCounter(ApplicationMetricsRegistry.AwardedBonusAmountCounter, Convert.ToInt64(command.Money));
				return commandResponse;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}