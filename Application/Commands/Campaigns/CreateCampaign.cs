using System.Collections.Generic;
using Template.Domain.Services;
using Template.Domain.Values;

namespace Template.Application.Commands.Campaigns
{
	public class CreateCampaignCommand : BaseCommand<long, CreateCampaignCommandResponse>
	{
		public override long Id { get; protected set; }
		public CampaignSpecification CampaignSpecification { get; }

		private CreateCampaignCommand(long id, CampaignSpecification campaignSpecification)
		{
			Id = id;
			CampaignSpecification = campaignSpecification;
		}

		public static CreateCampaignCommand Create(CampaignSpecification campaignSpecification)
		{
			return new CreateCampaignCommand(IdGenerator.Generate(), campaignSpecification);
		}
	}

	public class CreateCampaignCommandResponse : BaseCommandResponse<long>
	{
		public override long Id { get; protected set; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private CreateCampaignCommandResponse(long id) : base(id) { }

		private CreateCampaignCommandResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static CreateCampaignCommandResponse Create(long id)
		{
			return new CreateCampaignCommandResponse(id);
		}

		public static CreateCampaignCommandResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new CreateCampaignCommandResponse(id, errors);
		}
	}
}