using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Customers
{
	public class AddBonusMoneyCommand : BaseCommand<long, AddBonusMoneyCommandResponse>
	{
		public override long Id { get; protected set; }
		public long CustomerId { get; }
		public decimal Money { get; }

		private AddBonusMoneyCommand(long id, long customerId, decimal money)
		{
			Id = id;
			CustomerId = customerId;
			Money = money;
		}

		public static AddBonusMoneyCommand Create(long customerId, decimal money)
		{
			return new AddBonusMoneyCommand(IdGenerator.Generate(), customerId, money);
		}
	}

	public class AddBonusMoneyCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private AddBonusMoneyCommandResponse(long id) : base(id) { }

		private AddBonusMoneyCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static AddBonusMoneyCommandResponse Create(long id)
		{
			return new AddBonusMoneyCommandResponse(id);
		}

		public static AddBonusMoneyCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new AddBonusMoneyCommandResponse(id, errors);
		}
	}
}