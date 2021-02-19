using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Bonuses;
using Template.Persistence.Settings;

namespace Template.Persistence.RetrievalQueries
{
	internal class ExistingBonusWithSameCampaignAndCustomerRetrievalQuery : IQueryRetrievalPersistence<long, ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest, ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult>
	{
		private readonly string _customerDatabaseConnectionString;

		public ExistingBonusWithSameCampaignAndCustomerRetrievalQuery(DatabaseContextSettings databaseContextSettings)
		{
			_customerDatabaseConnectionString = databaseContextSettings.CustomerDatabaseConnectionString;
		}

		public async Task<ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult> Retrieve(ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest request)
		{
			using var dbConnection = new SqlConnection(_customerDatabaseConnectionString);
			const string sql = "select * from Bonuses where CampaignId = @campaignId and CustomerId = @customerId";
			var bonus = await dbConnection.QuerySingleOrDefaultAsync<Bonus>(sql, new { CampaignId = request.CampaignId, CustomerId = request.CustomerId });
			return ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult.Create(bonus != null);
		}
	}
}