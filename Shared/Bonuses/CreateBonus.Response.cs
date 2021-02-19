using System.Collections.Generic;

namespace Template.Shared.Bonuses
{
	public class CreateBonusResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private CreateBonusResponse() { }

		private CreateBonusResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static CreateBonusResponse Create()
		{
			return new CreateBonusResponse();
		}

		public static CreateBonusResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new CreateBonusResponse(errors);
		}
	}
}