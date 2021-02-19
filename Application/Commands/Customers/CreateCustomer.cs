using System;
using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Customers
{
	public class CreateCustomerCommand : BaseCommand<long, CreateCustomerCommandResponse>
	{
		public override long Id { get; protected set; }
		public string Name { get; }
		public string Email { get; }
		public DateTime DateOfBirth { get; }

		private CreateCustomerCommand(long id, string name, string email, DateTime dateOfBirth)
		{
			Id = id;
			Name = name;
			Email = email;
			DateOfBirth = dateOfBirth;
		}

		public static CreateCustomerCommand Create(string name, string email, DateTime dateOfBirth)
		{
			return new CreateCustomerCommand(IdGenerator.Generate(), name, email, dateOfBirth);
		}
	}

	public class CreateCustomerCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private CreateCustomerCommandResponse(long id) : base(id) { }

		private CreateCustomerCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static CreateCustomerCommandResponse Create(long id)
		{
			return new CreateCustomerCommandResponse(id);
		}

		public static CreateCustomerCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new CreateCustomerCommandResponse(id, errors);
		}
	}
}