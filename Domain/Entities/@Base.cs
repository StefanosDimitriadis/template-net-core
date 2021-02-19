using System;

namespace Template.Domain.Entities
{
	public abstract class BaseEntity<TId> : BaseEntity<TId>.IIdentifiable, BaseEntity<TId>.ICreatable, BaseEntity<TId>.IUpdatable, BaseEntity<TId>.IDeletable
	{
		public abstract TId Id { get; }
		public abstract DateTime CreatedAt { get; }
		public abstract DateTime? UpdatedAt { get; protected set; }
		public abstract bool IsDeleted { get; protected set; }

		protected interface IIdentifiable
		{
			TId Id { get; }
		}

		protected interface ICreatable
		{
			DateTime CreatedAt { get; }
		}

		protected interface IUpdatable
		{
			DateTime? UpdatedAt { get; }
		}

		protected interface IDeletable
		{
			bool IsDeleted { get; }
		}
	}
}