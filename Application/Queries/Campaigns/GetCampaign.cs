using System.Collections.Generic;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Services;

namespace Template.Application.Queries.Campaigns
{
	public class GetCampaignQuery : BaseQuery<long, GetCampaignQueryResponse>
	{
		public override long Id { get; protected set; }
		public long CampaignId { get; }

		private GetCampaignQuery(long id, long campaignId)
		{
			Id = id;
			CampaignId = campaignId;
		}

		public static GetCampaignQuery Create(long campaignId)
		{
			return new GetCampaignQuery(IdGenerator.Generate(), campaignId);
		}
	}

	public class GetCampaignQueryResponse : BaseQueryResponse<long>
	{
		public override long Id { get; protected set; }
		public Campaign Campaign { get; }
		public override IReadOnlyCollection<Error> Errors { get; protected set; }

		private GetCampaignQueryResponse(long id, Campaign campaign)
			: base(id)
		{
			Campaign = campaign;
		}

		private GetCampaignQueryResponse(long id, IReadOnlyCollection<Error> errors) : base(id, errors) { }

		public static GetCampaignQueryResponse Create(long id, Campaign campaign)
		{
			return new GetCampaignQueryResponse(id, campaign);
		}

		public static GetCampaignQueryResponse CreateError(long id, IReadOnlyCollection<Error> errors)
		{
			return new GetCampaignQueryResponse(id, errors);
		}
	}
}