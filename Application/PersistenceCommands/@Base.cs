using System.Collections.Generic;

namespace Template.Application.PersistenceCommands
{
	public abstract class BasePersistenceCommand<TId>
	{
		public abstract TId Id { get; protected set; }
	}

	public abstract class BasePersistenceCommandResponse<TId>
	{
		public abstract TId Id { get; protected set; }
		public abstract IReadOnlyCollection<Error> Errors { get; protected set; }

		protected BasePersistenceCommandResponse(TId id)
		{
			Id = id;
		}

		protected BasePersistenceCommandResponse(TId id, IReadOnlyCollection<Error> errors)
		{
			Id = id;
			Errors = errors;
		}
	}
}