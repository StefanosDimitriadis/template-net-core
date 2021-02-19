using System;

namespace Template.Shared.Customers
{
	public class CreateCustomerRequest
	{
		public string Name { get; }
		public string Email { get; }
		public DateTime DateOfBirth { get; }

		public CreateCustomerRequest(string name, string email, DateTime dateOfBirth)
		{
			Name = name;
			Email = email;
			DateOfBirth = dateOfBirth;
		}
	}
}