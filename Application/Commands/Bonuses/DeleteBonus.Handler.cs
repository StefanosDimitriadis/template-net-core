using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Bonuses;

namespace Template.Application.Commands.Bonuses
{
	internal class DeleteBonusCommandHandler : BaseCommandHandler<long, DeleteBonusCommand, DeleteBonusCommandResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Bonus> _entityRetrievalPersistence;
		private readonly IEntityModificationPersistence<long, long, Bonus, DeleteModification<long, long, Bonus>> _entityModificationPersistence;
		private readonly ILogger<DeleteBonusCommandHandler> _logger;

		public DeleteBonusCommandHandler(
			IEntityRetrievalPersistence<long, Bonus> entityRetrievalPersistence,
			IEntityModificationPersistence<long, long, Bonus, DeleteModification<long, long, Bonus>> entityModificationPersistence,
			ILogger<DeleteBonusCommandHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_entityModificationPersistence = entityModificationPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<DeleteBonusCommandResponse>> HandleInternal(DeleteBonusCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var bonus = await _entityRetrievalPersistence.Retrieve(command.BonusId);
				if (bonus == null)
					return new NotifiableResponse<DeleteBonusCommandResponse>(DeleteBonusCommandResponse.CreateError(command.Id, new Error[] { new Error($"Bonus with id: [{command.BonusId}] not found") }));
				if (bonus.IsDeleted)
					return new NotifiableResponse<DeleteBonusCommandResponse>(DeleteBonusCommandResponse.CreateError(command.Id, new Error[] { new Error($"Bonus with id: [{command.BonusId}] is already deleted") }));

				var modification = bonus.Delete();
				await _entityModificationPersistence.Persist(modification);
				return new NotifiableResponse<DeleteBonusCommandResponse>(DeleteBonusCommandResponse.Create(command.Id), modification.Events);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Bonus with id: [{bonusId}] not deleted", command.BonusId);
				return new NotifiableResponse<DeleteBonusCommandResponse>(DeleteBonusCommandResponse.CreateError(command.Id, new Error[] { new Error($"Bonus with id: [{command.BonusId}] not deleted") }));
			}
		}
	}
}