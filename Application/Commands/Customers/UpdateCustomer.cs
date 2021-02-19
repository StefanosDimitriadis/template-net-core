using System;
using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Customers
{
	public class UpdateCustomerCommand : BaseCommand<long, UpdateCustomerCommandResponse>
	{
		public override long Id { get; protected set; }
		public long CustomerId { get; }
		public string Email { get; }
		public DateTime DateOfBirth { get; }

		private UpdateCustomerCommand(long id, long customerId, string email, DateTime dateOfBirth)
		{
			Id = id;
			CustomerId = customerId;
			Email = email;
			DateOfBirth = dateOfBirth;
		}

		public static UpdateCustomerCommand Create(long customerId, string email, DateTime dateOfBirth)
		{
			return new UpdateCustomerCommand(IdGenerator.Generate(), customerId, email, dateOfBirth);
		}
	}

	public class UpdateCustomerCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private UpdateCustomerCommandResponse(long id) : base(id) { }

		private UpdateCustomerCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static UpdateCustomerCommandResponse Create(long id)
		{
			return new UpdateCustomerCommandResponse(id);
		}

		public static UpdateCustomerCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new UpdateCustomerCommandResponse(id, errors);
		}
	}
}