using System.Collections.Generic;

namespace Template.Shared.Customers
{
	public class GetCustomerResponse : BaseResponse
	{
		public override IReadOnlyCollection<Error> Errors { get; protected set; }
		public Customer Customer { get; }

		private GetCustomerResponse(Customer customer)
		{
			Customer = customer;
		}

		private GetCustomerResponse(IReadOnlyCollection<Error> errors) : base(errors) { }

		public static GetCustomerResponse Create(Customer customer)
		{
			return new GetCustomerResponse(customer);
		}

		public static GetCustomerResponse CreateError(IReadOnlyCollection<Error> errors)
		{
			return new GetCustomerResponse(errors);
		}
	}
}