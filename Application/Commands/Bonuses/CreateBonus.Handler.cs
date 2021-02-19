using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Bonuses;
using Template.Persistence.RetrievalQueries;

namespace Template.Application.Commands.Bonuses
{
	internal class CreateBonusCommandHandler : BaseCommandHandler<long, CreateBonusCommand, CreateBonusCommandResponse>
	{
		private readonly IQueryRetrievalPersistence<long, ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest, ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult> _queryRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Bonus, CreateModification<long, long, Bonus>> _entityModificationPersistence;
		private readonly ILogger<CreateBonusCommandHandler> _logger;

		public CreateBonusCommandHandler(
			IQueryRetrievalPersistence<long, ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest, ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult> queryRetrievalPersistence,
			IEntityModificationPersistence<long, long, Bonus, CreateModification<long, long, Bonus>> entityModificationPersistence,
			ILogger<CreateBonusCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_queryRetrievalPersistence = queryRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<CreateBonusCommandResponse>> HandleInternal(CreateBonusCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var retrievalQueryResult = await _queryRetrievalPersistence.Retrieve(ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest.Create(command.CampaignId, command.CustomerId));
				if (retrievalQueryResult.ExistingBonusWithSameCampaignAndCustomer)
					return new NotifiableResponse<CreateBonusCommandResponse>(CreateBonusCommandResponse.CreateError(command.Id, new Error[] { new Error("Bonus already exists") }));

				var modification = Bonus.Create(command.CampaignId, command.CustomerId);
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<CreateBonusCommandResponse>(CreateBonusCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Bonus not created");
				return new NotifiableResponse<CreateBonusCommandResponse>(CreateBonusCommandResponse.CreateError(command.Id, new Error[] { new Error("Bonus not created") }));
			}
		}
	}
}