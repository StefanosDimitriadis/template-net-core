﻿using Dapper;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Bonuses;
using Template.Persistence.Services;
using Template.Persistence.Settings;

namespace Template.Persistence.RetrievalQueries
{
	internal class ExistingBonusWithSameCampaignAndCustomerRetrievalQuery : IQueryRetrievalPersistence<long, ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest, ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult>
	{
		private readonly IDbConnectionService _dbConnectionService;
		private readonly int _bonusDatabaseTimeoutInSeconds;

		public ExistingBonusWithSameCampaignAndCustomerRetrievalQuery(
			IDbConnectionService dbConnectionService,
			DatabaseContextSettings databaseContextSettings)
		{
			_dbConnectionService = dbConnectionService;
			_bonusDatabaseTimeoutInSeconds = databaseContextSettings.BonusDatabaseTimeoutInSeconds;
		}

		public async Task<ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult> Retrieve(ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest request)
		{
			using var dbConnection = _dbConnectionService.CreateBonusDatabaseSqlConnection();
			const string sql = "select * from Bonuses where CampaignId = @campaignId and CustomerId = @customerId";
			var bonus = await dbConnection.QuerySingleOrDefaultAsync<Bonus>(sql, new { CampaignId = request.CampaignId, CustomerId = request.CustomerId }, commandTimeout: _bonusDatabaseTimeoutInSeconds);
			return ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult.Create(bonus != null);
		}
	}
}