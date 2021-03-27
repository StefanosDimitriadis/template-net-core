using Dapper;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Customers;
using Template.Persistence.Services;
using Template.Persistence.Settings;

namespace Template.Persistence.RetrievalQueries
{
	internal class ActiveCustomersRetrievalQuery : IQueryRetrievalPersistence<long, ActiveCustomersRetrievalQueryResult>
	{
		private readonly IDbConnectionService _dbConnectionService;
		private readonly int _customerDatabaseTimeoutInSeconds;

		public ActiveCustomersRetrievalQuery(
			IDbConnectionService dbConnectionService,
			DatabaseContextSettings databaseContextSettings)
		{
			_dbConnectionService = dbConnectionService;
			_customerDatabaseTimeoutInSeconds = databaseContextSettings.CustomerDatabaseTimeoutInSeconds;
		}

		public async Task<ActiveCustomersRetrievalQueryResult> Retrieve()
		{
			using var dbConnection = _dbConnectionService.CreateCustomerDatabaseSqlConnection();
			const string sql = "select * from Customers where IsDeleted = @IsDeleted";
			var activeCustomers = await dbConnection.QueryAsync<Customer>(sql, new { IsDeleted = false }, commandTimeout: _customerDatabaseTimeoutInSeconds);
			return ActiveCustomersRetrievalQueryResult.Create(new ReadOnlyCollection<Customer>(activeCustomers.AsList()));
		}
	}
}