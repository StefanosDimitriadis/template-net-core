using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Campaigns;

namespace Template.Application.Queries.Campaigns
{
	internal class GetCampaignQueryHandler : BaseQueryHandler<long, GetCampaignQuery, GetCampaignQueryResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Campaign> _entityRetrievalPersistence;
		private readonly ILogger<GetCampaignQueryHandler> _logger;

		public GetCampaignQueryHandler(
			IEntityRetrievalPersistence<long, Campaign> entityRetrievalPersistence,
			ILogger<GetCampaignQueryHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<GetCampaignQueryResponse>> HandleInternal(GetCampaignQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var campaign = await _entityRetrievalPersistence.Retrieve(query.CampaignId);
				if (campaign == null)
					return GetErrorResponse(query.Id, query.CampaignId);

				return new NotifiableResponse<GetCampaignQueryResponse>(GetCampaignQueryResponse.Create(query.Id, campaign));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Campaign with id: [{campaignId}] not found", query.CampaignId);
				return GetErrorResponse(query.Id, query.CampaignId);
			}
		}

		private NotifiableResponse<GetCampaignQueryResponse> GetErrorResponse(long queryId, long campaignId)
		{
			return new NotifiableResponse<GetCampaignQueryResponse>(GetCampaignQueryResponse.CreateError(queryId, new Error[] { new Error($"Campaign with id: [{campaignId}] not found") }));
		}
	}
}