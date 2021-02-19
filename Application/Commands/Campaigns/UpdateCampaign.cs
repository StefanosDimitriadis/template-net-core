using System.Collections.Generic;
using Template.Domain.Services;
using Template.Domain.Values;

namespace Template.Application.Commands.Campaigns
{
	public class UpdateCampaignCommand : BaseCommand<long, UpdateCampaignCommandResponse>
	{
		public override long Id { get; protected set; }
		public long CampaignId { get; }
		public CampaignSpecification CampaignSpecification { get; }

		private UpdateCampaignCommand(long id, long campaignId, CampaignSpecification campaignSpecification)
		{
			Id = id;
			CampaignId = campaignId;
			CampaignSpecification = campaignSpecification;
		}

		public static UpdateCampaignCommand Create(long campaignId, CampaignSpecification campaignSpecification)
		{
			return new UpdateCampaignCommand(IdGenerator.Generate(), campaignId, campaignSpecification);
		}
	}

	public class UpdateCampaignCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private UpdateCampaignCommandResponse(long id) : base(id) { }

		private UpdateCampaignCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static UpdateCampaignCommandResponse Create(long id)
		{
			return new UpdateCampaignCommandResponse(id);
		}

		public static UpdateCampaignCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new UpdateCampaignCommandResponse(id, errors);
		}
	}
}