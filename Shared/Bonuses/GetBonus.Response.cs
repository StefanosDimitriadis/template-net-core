using System.Collections.Generic;

namespace Template.Shared.Bonuses
{
	public class GetBonusResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }
		public Bonus Bonus { get; }

		private GetBonusResponse(Bonus bonus)
		{
			Bonus = bonus;
		}

		private GetBonusResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static GetBonusResponse Create(Bonus bonus)
		{
			return new GetBonusResponse(bonus);
		}

		public static GetBonusResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new GetBonusResponse(errors);
		}
	}
}