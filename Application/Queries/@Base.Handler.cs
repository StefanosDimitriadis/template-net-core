using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Queries;

namespace Template.Application.Queries
{
	internal abstract class BaseQueryHandler<TId, TQuery, TQueryResponse> : IRequestHandler<TQuery, TQueryResponse>
		where TQuery : BaseQuery<TId, TQueryResponse>
		where TQueryResponse : BaseQueryResponse<TId>
	{
		private readonly IMediator _mediator;

		public async Task<TQueryResponse> Handle(TQuery query, CancellationToken cancellationToken)
		{
			var notifiableResponse = await HandleInternal(query, cancellationToken);

			if (notifiableResponse.Events?.Count > 0)
				foreach (var @event in notifiableResponse.Events)
					await _mediator.Publish(@event, cancellationToken);

			return notifiableResponse.Response;
		}

		protected abstract Task<NotifiableResponse<TQueryResponse>> HandleInternal(TQuery query, CancellationToken cancellationToken);

		protected BaseQueryHandler(IMediator mediator)
		{
			_mediator = mediator;
		}
	}
}