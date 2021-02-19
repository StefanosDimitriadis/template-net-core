using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Application.Commands
{
	internal abstract class BaseCommandHandler<TId, TCommand, TCommandResponse> : IRequestHandler<TCommand, TCommandResponse>
		where TCommand : BaseCommand<TId, TCommandResponse>
		where TCommandResponse : BaseCommandResponse<TId>
	{
		private readonly IMediator _mediator;

		protected BaseCommandHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<TCommandResponse> Handle(TCommand command, CancellationToken cancellationToken)
		{
			var notifiableResponse = await HandleInternal(command, cancellationToken);

			if (notifiableResponse.Events?.Count > 0)
				foreach (var @event in notifiableResponse.Events)
					await _mediator.Publish(@event, cancellationToken);

			return notifiableResponse.Response;
		}

		protected abstract Task<NotifiableResponse<TCommandResponse>> HandleInternal(TCommand command, CancellationToken cancellationToken);
	}
}