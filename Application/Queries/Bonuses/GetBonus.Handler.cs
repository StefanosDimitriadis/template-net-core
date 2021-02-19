using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Bonuses;

namespace Template.Application.Queries.Bonuses
{
	internal class GetBonusQueryHandler : BaseQueryHandler<long, GetBonusQuery, GetBonusQueryResponse>
	{
		private readonly IEntityRetrievalPersistence<long, Bonus> _entityRetrievalPersistence;
		private readonly ILogger<GetBonusQueryHandler> _logger;

		public GetBonusQueryHandler(
			IEntityRetrievalPersistence<long, Bonus> entityRetrievalPersistence,
			ILogger<GetBonusQueryHandler> logger,
			IMediator mediator)
			: base(mediator)
		{
			_entityRetrievalPersistence = entityRetrievalPersistence;
			_logger = logger;
		}

		protected override async Task<NotifiableResponse<GetBonusQueryResponse>> HandleInternal(GetBonusQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var bonus = await _entityRetrievalPersistence.Retrieve(query.BonusId);
				if (bonus == null)
					return GetErrorResponse(query.Id, query.BonusId);

				return new NotifiableResponse<GetBonusQueryResponse>(GetBonusQueryResponse.Create(query.Id, bonus));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Bonus with id: [{bonusId}] not found", query.BonusId);
				return GetErrorResponse(query.Id, query.BonusId);
			}
		}

		private NotifiableResponse<GetBonusQueryResponse> GetErrorResponse(long queryId, long bonusId)
		{
			return new NotifiableResponse<GetBonusQueryResponse>(GetBonusQueryResponse.CreateError(queryId, new Error[] { new Error($"Bonus with id: [{bonusId}] not found") }));
		}
	}
}