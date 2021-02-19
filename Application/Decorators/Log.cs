using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Commands;
using Template.Application.Queries;

namespace Template.Application.Decorators
{
	internal class LogQueryDecorator<TQuery, TQueryResponse> : IRequestHandler<TQuery, TQueryResponse>
		where TQuery : IRequest<TQueryResponse>
	{
		private readonly IRequestHandler<TQuery, TQueryResponse> _requestHandler;
		private readonly ILogger<LogQueryDecorator<TQuery, TQueryResponse>> _logger;

		public LogQueryDecorator(
			IRequestHandler<TQuery, TQueryResponse> requestHandler,
			ILogger<LogQueryDecorator<TQuery, TQueryResponse>> logger)
		{
			_requestHandler = requestHandler;
			_logger = logger;
		}

		public async Task<TQueryResponse> Handle(TQuery query, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Handling query with name: [{name}] and id: [{id}]", typeof(TQuery).Name, (query as BaseQuery<long, TQueryResponse>).Id);
				var queryResponse = await _requestHandler.Handle(query, cancellationToken);
				_logger.LogInformation("Handled query with name: [{name}] and id: [{id}]", typeof(TQuery).Name, (query as BaseQuery<long, TQueryResponse>).Id);
				return queryResponse;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	internal class LogCommandDecorator<TCommand, TCommandResponse> : IRequestHandler<TCommand, TCommandResponse>
		where TCommand : IRequest<TCommandResponse>
	{
		private readonly IRequestHandler<TCommand, TCommandResponse> _requestHandler;
		private readonly ILogger<LogCommandDecorator<TCommand, TCommandResponse>> _logger;

		public LogCommandDecorator(
			IRequestHandler<TCommand, TCommandResponse> requestHandler,
			ILogger<LogCommandDecorator<TCommand, TCommandResponse>> logger)
		{
			_requestHandler = requestHandler;
			_logger = logger;
		}

		public async Task<TCommandResponse> Handle(TCommand command, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Handling command with name: [{name}] and id: [{id}]", typeof(TCommand).Name, (command as BaseCommand<long, TCommandResponse>).Id);
				var commandResponse = await _requestHandler.Handle(command, cancellationToken);
				_logger.LogInformation("Handled command with name: [{name}] and id: [{id}]", typeof(TCommand).Name, (command as BaseCommand<long, TCommandResponse>).Id);
				return commandResponse;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}