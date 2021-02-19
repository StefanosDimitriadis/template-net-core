using MediatR;
using System.Collections.Generic;

namespace Template.Application.Commands
{
	public abstract class BaseCommand<TId, TResponse> : IRequest<TResponse>
	{
		public abstract TId Id { get; protected set; }
	}

	public abstract class BaseCommandResponse<TId>
	{
		public abstract TId Id { get; protected set; }
		public abstract IReadOnlyCollection<Error> Errors { get; protected set; }

		protected BaseCommandResponse(TId id)
		{
			Id = id;
		}

		protected BaseCommandResponse(TId id, IReadOnlyCollection<Error> errors)
		{
			Id = id;
			Errors = errors;
		}
	}
}