using System.Collections.Generic;
using Template.Domain.Entities.Bonuses;
using Template.Domain.Services;

namespace Template.Application.Queries.Bonuses
{
	public class GetBonusQuery : BaseQuery<long, GetBonusQueryResponse>
	{
		public override long Id { get; protected set; }
		public long BonusId { get; }

		private GetBonusQuery(long id, long bonusId)
		{
			Id = id;
			BonusId = bonusId;
		}

		public static GetBonusQuery Create(long bonusId)
		{
			return new GetBonusQuery(IdGenerator.Generate(), bonusId);
		}
	}

	public class GetBonusQueryResponse : BaseQueryResponse<long>
	{
		public override long Id { get; protected set; }
		public Bonus Bonus { get; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private GetBonusQueryResponse(long id, Bonus bonus)
			: base(id)
		{
			Bonus = bonus;
		}

		private GetBonusQueryResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static GetBonusQueryResponse Create(long id, Bonus bonus)
		{
			return new GetBonusQueryResponse(id, bonus);
		}

		public static GetBonusQueryResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new GetBonusQueryResponse(id, errors);
		}
	}
}