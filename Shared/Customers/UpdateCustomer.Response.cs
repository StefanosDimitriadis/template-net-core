using System.Collections.Generic;

namespace Template.Shared.Customers
{
	public class UpdateCustomerResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private UpdateCustomerResponse() { }

		private UpdateCustomerResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static UpdateCustomerResponse Create()
		{
			return new UpdateCustomerResponse();
		}

		public static UpdateCustomerResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new UpdateCustomerResponse(errors);
		}
	}
}