using System.Collections.Generic;
using Template.Domain.Entities;

namespace Template.Application
{
	internal class NotifiableResponse<TResponse>
	{
		internal TResponse Response { get; }
		internal IReadOnlyCollection<BaseEvent<long, long>> Events { get; }

		internal NotifiableResponse(TResponse response, IReadOnlyCollection<BaseEvent<long, long>> events = null)
		{
			Response = response;
			Events = events;
		}
	}
}