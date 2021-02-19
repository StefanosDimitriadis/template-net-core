using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Bonuses;

namespace Template.Application.Commands.Bonuses
{
	internal class AwardBonusCommandHandler : BaseCommandHandler<long, AwardBonusCommand, AwardBonusCommandResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Bonus> _bonusRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Bonus, AwardModification> _entityModificationPersistence;
		private readonly ILogger<AwardBonusCommandHandler> _logger;

		public AwardBonusCommandHandler(
			IEntityRetrievalPersistence<long, Bonus> bonusRetrievalPersistence,
			IEntityModificationPersistence<long, long, Bonus, AwardModification> entityModificationPersistence,
			ILogger<AwardBonusCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_bonusRetrievalPersistence = bonusRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<AwardBonusCommandResponse>> HandleInternal(AwardBonusCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var bonus = await _bonusRetrievalPersistence.Retrieve(command.BonusId);
				if (bonus == null)
					return new NotifiableResponse<AwardBonusCommandResponse>(AwardBonusCommandResponse.CreateError(command.Id, new Error[] { new Error($"Bonus with id: [{command.BonusId}] not found") }));
				if (bonus.HasBeenAwarded)
					return new NotifiableResponse<AwardBonusCommandResponse>(AwardBonusCommandResponse.CreateError(command.Id, new Error[] { new Error($"Bonus with id: [{command.BonusId}] has been already awarded") }));

				var modification = bonus.Award();
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<AwardBonusCommandResponse>(AwardBonusCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Bonus with id: [{bonusId}] not awarded", command.BonusId);
				return new NotifiableResponse<AwardBonusCommandResponse>(AwardBonusCommandResponse.CreateError(command.Id, new Error[] { new Error($"Bonus with id: [{command.BonusId}] not awarded") }));
			}
		}
	}
}