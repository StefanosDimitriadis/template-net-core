using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Customers
{
	public class AddMoneyCommand : BaseCommand<long, AddMoneyCommandResponse>
	{
		public override long Id { get; protected set; }
		public long CustomerId { get; }
		public decimal Money { get; }

		private AddMoneyCommand(long id, long customerId, decimal money)
		{
			Id = id;
			CustomerId = customerId;
			Money = money;
		}

		public static AddMoneyCommand Create(long customerId, decimal money)
		{
			return new AddMoneyCommand(IdGenerator.Generate(), customerId, money);
		}
	}

	public class AddMoneyCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private AddMoneyCommandResponse(long id) : base(id) { }

		private AddMoneyCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static AddMoneyCommandResponse Create(long id)
		{
			return new AddMoneyCommandResponse(id);
		}

		public static AddMoneyCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new AddMoneyCommandResponse(id, errors);
		}
	}
}