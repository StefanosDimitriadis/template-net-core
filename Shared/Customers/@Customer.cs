using System;

namespace Template.Shared.Customers
{
	public class Customer
	{
		public long Id { get; }
		public string Name { get; }
		public string Email { get; }
		public DateTime DateOfBirth { get; }
		public decimal Money { get; }
		public DateTime CreatedAt { get; }
		public DateTime? UpdatedAt { get; }
		public bool IsDeleted { get; }

		public Customer(long id, string name, string email, DateTime dateOfBirth, decimal money, DateTime createdAt, DateTime? updatedAt, bool isDeleted)
		{
			Id = id;
			Name = name;
			Email = email;
			DateOfBirth = dateOfBirth;
			Money = money;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			IsDeleted = isDeleted;
		}
	}
}