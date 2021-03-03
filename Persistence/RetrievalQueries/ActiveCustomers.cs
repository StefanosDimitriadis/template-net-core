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
		private readonly ISqlConnectionService _sqlConnectionService;
		private readonly int _customerDatabaseTimeoutInSeconds;

		public ActiveCustomersRetrievalQuery(
			ISqlConnectionService sqlConnectionService,
			DatabaseContextSettings databaseContextSettings)
		{
			_sqlConnectionService = sqlConnectionService;
			_customerDatabaseTimeoutInSeconds = databaseContextSettings.CustomerDatabaseTimeoutInSeconds;
		}

		public async Task<ActiveCustomersRetrievalQueryResult> Retrieve()
		{
			using var dbConnection = _sqlConnectionService.CreateCustomerDatabaseSqlConnection();
			const string sql = "select * from Customers where IsDeleted = @IsDeleted";
			var activeCustomers = await dbConnection.QueryAsync<Customer>(sql, new { IsDeleted = false }, commandTimeout: _customerDatabaseTimeoutInSeconds);
			return ActiveCustomersRetrievalQueryResult.Create(new ReadOnlyCollection<Customer>(activeCustomers.AsList()));
		}
	}
}