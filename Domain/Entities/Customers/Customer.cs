using Newtonsoft.Json;
using System;
using Template.Domain.Services;

namespace Template.Domain.Entities.Customers
{
	public class Customer : BaseEntity<long>
	{
		public override long Id { get; }
		public string Name { get; }
		public string Email { get; private set; }
		public DateTime DateOfBirth { get; private set; }
		public decimal Money { get; private set; }
		public override DateTime CreatedAt { get; }
		public override DateTime? UpdatedAt { get; protected set; }
		public override bool IsDeleted { get; protected set; }

		private Customer() { }

		[JsonConstructor]
		private Customer(long id, string name, string email, DateTime dateOfBirth, decimal money, DateTime createdAt, DateTime? updatedAt, bool isDeleted)
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

		private Customer(long id, string name, string email, DateTime dateOfBirth, DateTime createdAt)
		{
			Id = id;
			Name = name;
			Email = email;
			DateOfBirth = dateOfBirth;
			CreatedAt = createdAt;
		}

		public static CreateModification<long, long, Customer> Create(string name, string email, DateTime dateOfBirth)
		{
			var customer = new Customer(IdGenerator.Generate(), name, email, dateOfBirth, DateTime.UtcNow);
			var events = new BaseEvent<long, long>[]
			{
				CustomerCreatedEvent.Create(customer.Id)
			};
			return new CreateModification<long, long, Customer>(customer, events);
		}

		public UpdateModification<long, long, Customer> Update(string email, DateTime dateOfBirth)
		{
			Email = email;
			DateOfBirth = dateOfBirth;
			UpdatedAt = DateTime.UtcNow;
			var events = new BaseEvent<long, long>[]
			{
				CustomerUpdatedEvent.Create(Id)
			};
			return new UpdateModification<long, long, Customer>(this, events);
		}

		public AddMoneyModification AddMoney(decimal money)
		{
			var initialMoney = Money;
			Money += money;
			UpdatedAt = DateTime.UtcNow;
			var events = new BaseEvent<long, long>[]
			{
				CustomerMoneyAddedEvent.Create(Id, initialMoney, moneyToAdd: money, finalMoney: Money)
			};
			return new AddMoneyModification(this, events);
		}

		public DeleteModification<long, long, Customer> Delete()
		{
			UpdatedAt = DateTime.UtcNow;
			IsDeleted = true;
			var events = new BaseEvent<long, long>[]
			{
				CustomerDeletedEvent.Create(Id)
			};
			return new DeleteModification<long, long, Customer>(this, events);
		}
	}
}