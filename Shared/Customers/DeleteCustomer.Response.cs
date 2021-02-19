using System.Collections.Generic;

namespace Template.Shared.Customers
{
	public class DeleteCustomerResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private DeleteCustomerResponse() { }

		private DeleteCustomerResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static DeleteCustomerResponse Create()
		{
			return new DeleteCustomerResponse();
		}

		public static DeleteCustomerResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new DeleteCustomerResponse(errors);
		}
	}
}