using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Customers
{
	public class DeleteCustomerCommand : BaseCommand<long, DeleteCustomerCommandResponse>
	{
		public override long Id { get; protected set; }
		public long CustomerId { get; }

		private DeleteCustomerCommand(long id, long customerId)
		{
			Id = id;
			CustomerId = customerId;
		}

		public static DeleteCustomerCommand Create(long customerId)
		{
			return new DeleteCustomerCommand(IdGenerator.Generate(), customerId);
		}
	}

	public class DeleteCustomerCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private DeleteCustomerCommandResponse(long id) : base(id) { }

		private DeleteCustomerCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static DeleteCustomerCommandResponse Create(long id)
		{
			return new DeleteCustomerCommandResponse(id);
		}

		public static DeleteCustomerCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new DeleteCustomerCommandResponse(id, errors);
		}
	}
}