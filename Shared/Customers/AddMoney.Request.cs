using Newtonsoft.Json;

namespace Template.Shared.Customers
{
	public class AddMoneyRequest
	{
		[JsonIgnore]
		public long CustomerId { get; private set; }
		public decimal Money { get; }

		public AddMoneyRequest(decimal money)
		{
			Money = money;
		}

		public void SetCustomerId(long customerId)
		{
			CustomerId = customerId;
		}
	}
}