namespace Template.Shared.Customers
{
	public class GetCustomerRequest
	{
		public long Id { get; }

		public GetCustomerRequest(long id)
		{
			Id = id;
		}
	}
}