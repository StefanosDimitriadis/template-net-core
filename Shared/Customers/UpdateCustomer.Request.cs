using Newtonsoft.Json;
using System;

namespace Template.Shared.Customers
{
	public class UpdateCustomerRequest
	{
		[JsonIgnore]
		public long Id { get; private set; }
		public string Email { get; }
		public DateTime DateOfBirth { get; }

		public UpdateCustomerRequest(string email, DateTime dateOfBirth)
		{
			Email = email;
			DateOfBirth = dateOfBirth;
		}

		public void SetId(long id)
		{
			Id = id;
		}
	}
}