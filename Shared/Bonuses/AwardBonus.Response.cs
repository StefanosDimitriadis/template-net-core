using System.Collections.Generic;

namespace Template.Shared.Bonuses
{
	public class AwardBonusResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private AwardBonusResponse() { }

		private AwardBonusResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static AwardBonusResponse Create()
		{
			return new AwardBonusResponse();
		}

		public static AwardBonusResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new AwardBonusResponse(errors);
		}
	}
}