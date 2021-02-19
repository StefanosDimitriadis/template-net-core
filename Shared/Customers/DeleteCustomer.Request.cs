namespace Template.Shared.Customers
{
	public class DeleteCustomerRequest
	{
		public long Id { get; }

		public DeleteCustomerRequest(long id)
		{
			Id = id;
		}
	}
}