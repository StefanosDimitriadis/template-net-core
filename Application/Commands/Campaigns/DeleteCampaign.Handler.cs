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
	internal class DeleteCampaignCommandHandler : BaseCommandHandler<long, DeleteCampaignCommand, DeleteCampaignCommandResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Campaign> _entityRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Campaign, DeleteModification<long, long, Campaign>> _entityModificationPersistence;
		private readonly ILogger<DeleteCampaignCommandHandler> _logger;

		public DeleteCampaignCommandHandler(
			IEntityRetrievalPersistence<long, Campaign> entityRetrievalPersistence,
			IEntityModificationPersistence<long, long, Campaign, DeleteModification<long, long, Campaign>> entityModificationPersistence,
			ILogger<DeleteCampaignCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<DeleteCampaignCommandResponse>> HandleInternal(DeleteCampaignCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var campaign = await _entityRetrievalPersistence.Retrieve(command.CampaignId);
				if (campaign == null)
					return new NotifiableResponse<DeleteCampaignCommandResponse>(DeleteCampaignCommandResponse.CreateError(command.Id, new Error[] { new Error($"Campaign with id: [{command.CampaignId}] not found") }));
				if (campaign.IsDeleted)
					return new NotifiableResponse<DeleteCampaignCommandResponse>(DeleteCampaignCommandResponse.CreateError(command.Id, new Error[] { new Error($"Campaign with id: [{command.CampaignId}] is already deleted") }));

				var modification = campaign.Delete();
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<DeleteCampaignCommandResponse>(DeleteCampaignCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Campaign with id: [{campaignId}] not deleted", command.CampaignId);
				return new NotifiableResponse<DeleteCampaignCommandResponse>(DeleteCampaignCommandResponse.CreateError(command.Id, new Error[] { new Error($"Campaign with id: [{command.CampaignId}] not deleted") }));
			}
		}
	}
}