using MediatR;
using System;

namespace Template.Domain.Entities
{
	public abstract class BaseEvent<TId, TEntityId>
		: INotification
	{
		public TId Id { get; protected set; }
		public TEntityId EntityId { get; protected set; }
		public DateTime CreatedAt { get; protected set; }
		public bool IsHandled { get; protected set; }
		public DateTime? HandledAt { get; protected set; }
		public string Type
		{
			get
			{
				return GetType().FullName;
			}
		}

		public abstract void SetHandled();
	}
}