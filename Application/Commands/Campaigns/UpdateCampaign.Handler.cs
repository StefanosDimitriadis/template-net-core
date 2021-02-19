using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Campaigns;

namespace Template.Application.Commands.Campaigns
{
	internal class UpdateCampaignCommandHandler : BaseCommandHandler<long, UpdateCampaignCommand, UpdateCampaignCommandResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Campaign> _entityRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Campaign, UpdateModification<long, long, Campaign>> _entityModificationPersistence;
		private readonly ILogger<UpdateCampaignCommandHandler> _logger;

		public UpdateCampaignCommandHandler(
			IEntityRetrievalPersistence<long, Campaign> entityRetrievalPersistence,
			IEntityModificationPersistence<long, long, Campaign, UpdateModification<long, long, Campaign>> entityModificationPersistence,
			ILogger<UpdateCampaignCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<UpdateCampaignCommandResponse>> HandleInternal(UpdateCampaignCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var campaign = await _entityRetrievalPersistence.Retrieve(command.CampaignId);
				if (campaign == null)
					return new NotifiableResponse<UpdateCampaignCommandResponse>(UpdateCampaignCommandResponse.CreateError(command.Id, new Error[] { new Error($"Campaign with id: [{command.CampaignId}] not found") }));
				if (campaign.CampaignSpecification == command.CampaignSpecification)
					return new NotifiableResponse<UpdateCampaignCommandResponse>(UpdateCampaignCommandResponse.CreateError(command.Id, new Error[] { new Error($"Campaign with id: [{command.CampaignId}] not updated due to same values") }));

				var modification = campaign.Update(command.CampaignSpecification);
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<UpdateCampaignCommandResponse>(UpdateCampaignCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Campaign with id: [{campaignId}] not updated", command.CampaignId);
				return new NotifiableResponse<UpdateCampaignCommandResponse>(UpdateCampaignCommandResponse.CreateError(command.Id, new Error[] { new Error($"Campaign with id: [{command.CampaignId}] not updated") }));
			}
		}
	}
}