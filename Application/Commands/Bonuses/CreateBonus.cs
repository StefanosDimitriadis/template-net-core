using System;
using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Bonuses
{
	public class CreateBonusCommand : BaseCommand<long, CreateBonusCommandResponse>
	{
		public override long Id { get; protected set; }
		public long CampaignId { get; }
		public long CustomerId { get; }

		private CreateBonusCommand(long id, long campaignId, long customerId)
		{
			Id = id;
			CampaignId = campaignId;
			CustomerId = customerId;
		}

		public static CreateBonusCommand Create(long campaignId, long customerId)
		{
			return new CreateBonusCommand(IdGenerator.Generate(), campaignId, customerId);
		}
	}

	public class CreateBonusCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private CreateBonusCommandResponse(long id) : base(id) { }

		private CreateBonusCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static CreateBonusCommandResponse Create(long id)
		{
			return new CreateBonusCommandResponse(id);
		}

		public static CreateBonusCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new CreateBonusCommandResponse(id, errors);
		}
	}
}