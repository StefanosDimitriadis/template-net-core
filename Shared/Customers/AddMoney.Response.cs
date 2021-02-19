using System.Collections.Generic;

namespace Template.Shared.Customers
{
	public class AddMoneyResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private AddMoneyResponse() { }

		private AddMoneyResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static AddMoneyResponse Create()
		{
			return new AddMoneyResponse();
		}

		public static AddMoneyResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new AddMoneyResponse(errors);
		}
	}
}