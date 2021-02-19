using MediatR;
using System.Collections.Generic;

namespace Template.Application.Queries
{
	public abstract class BaseQuery<TId, TResponse> : IRequest<TResponse>
	{
		public abstract TId Id { get; protected set; }
	}

	public abstract class BaseQueryResponse<TId>
	{
		public abstract TId Id { get; protected set; }
		public abstract IReadOnlyCollection<Error> Errors { get; protected set; }

		protected BaseQueryResponse(TId id)
		{
			Id = id;
		}

		protected BaseQueryResponse(TId id, IReadOnlyCollection<Error> errors)
		{
			Id = id;
			Errors = errors;
		}
	}
}