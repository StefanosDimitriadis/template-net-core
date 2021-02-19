using System.Collections.Generic;
using Template.Domain.Services;

namespace Template.Application.Commands.Campaigns
{
	public class DeleteCampaignCommand : BaseCommand<long, DeleteCampaignCommandResponse>
	{
		public override long Id { get; protected set; }
		public long CampaignId { get; }

		private DeleteCampaignCommand(long id, long campaignId)
		{
			Id = id;
			CampaignId = campaignId;
		}

		public static DeleteCampaignCommand Create(long campaignId)
		{
			return new DeleteCampaignCommand(IdGenerator.Generate(), campaignId);
		}
	}

	public class DeleteCampaignCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private DeleteCampaignCommandResponse(long id) : base(id) { }

		private DeleteCampaignCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static DeleteCampaignCommandResponse Create(long id)
		{
			return new DeleteCampaignCommandResponse(id);
		}

		public static DeleteCampaignCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new DeleteCampaignCommandResponse(id, errors);
		}
	}
}