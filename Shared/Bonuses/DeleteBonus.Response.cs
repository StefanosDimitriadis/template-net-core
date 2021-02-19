using System.Collections.Generic;

namespace Template.Shared.Bonuses
{
	public class DeleteBonusResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private DeleteBonusResponse() { }

		private DeleteBonusResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static DeleteBonusResponse Create()
		{
			return new DeleteBonusResponse();
		}

		public static DeleteBonusResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new DeleteBonusResponse(errors);
		}
	}
}