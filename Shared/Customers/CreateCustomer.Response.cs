using System.Collections.Generic;

namespace Template.Shared.Customers
{
	public class CreateCustomerResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private CreateCustomerResponse() { }

		private CreateCustomerResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static CreateCustomerResponse Create()
		{
			return new CreateCustomerResponse();
		}

		public static CreateCustomerResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new CreateCustomerResponse(errors);
		}
	}
}