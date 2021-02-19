using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Bonuses
{
	public class DeleteBonusCommand : BaseCommand<long, DeleteBonusCommandResponse>
	{
		public override long Id { get; protected set; }
		public long BonusId { get; }

		private DeleteBonusCommand(long id, long bonusId)
		{
			Id = id;
			BonusId = bonusId;
		}

		public static DeleteBonusCommand Create(long bonusId)
		{
			return new DeleteBonusCommand(IdGenerator.Generate(), bonusId);
		}
	}

	public class DeleteBonusCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private DeleteBonusCommandResponse(long id) : base(id) { }

		private DeleteBonusCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static DeleteBonusCommandResponse Create(long id)
		{
			return new DeleteBonusCommandResponse(id);
		}

		public static DeleteBonusCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new DeleteBonusCommandResponse(id, errors);
		}
	}
}