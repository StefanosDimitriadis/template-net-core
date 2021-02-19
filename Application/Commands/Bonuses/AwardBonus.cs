using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Bonuses
{
	public class AwardBonusCommand : BaseCommand<long, AwardBonusCommandResponse>
	{
		public override long Id { get; protected set; }
		public long BonusId { get; }

		private AwardBonusCommand(long id, long bonusId)
		{
			Id = id;
			BonusId = bonusId;
		}

		public static AwardBonusCommand Create(long bonusId)
		{
			return new AwardBonusCommand(IdGenerator.Generate(), bonusId);
		}
	}

	public class AwardBonusCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private AwardBonusCommandResponse(long id) : base(id) { }

		private AwardBonusCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static AwardBonusCommandResponse Create(long id)
		{
			return new AwardBonusCommandResponse(id);
		}

		public static AwardBonusCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new AwardBonusCommandResponse(id, errors);
		}
	}
}