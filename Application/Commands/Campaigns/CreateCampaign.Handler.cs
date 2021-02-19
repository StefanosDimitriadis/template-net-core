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
	internal class CreateCampaignCommandHandler : BaseCommandHandler<long, CreateCampaignCommand, CreateCampaignCommandResponse>
	{
		private readonly IEntityModificationPersistence<long, long, Campaign, CreateModification<long, long, Campaign>> _entityModificationPersistence;
		private readonly ILogger<CreateCampaignCommandHandler> _logger;

		public CreateCampaignCommandHandler(
			IEntityModificationPersistence<long, long, Campaign, CreateModification<long, long, Campaign>> entityModificationPersistence,
			ILogger<CreateCampaignCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<CreateCampaignCommandResponse>> HandleInternal(CreateCampaignCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var modification = Campaign.Create(command.CampaignSpecification);
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<CreateCampaignCommandResponse>(CreateCampaignCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Campaign not created");
				return new NotifiableResponse<CreateCampaignCommandResponse>(CreateCampaignCommandResponse.CreateError(command.Id, new Error[] { new Error("Campaign not created") }));
			}
		}
	}
}