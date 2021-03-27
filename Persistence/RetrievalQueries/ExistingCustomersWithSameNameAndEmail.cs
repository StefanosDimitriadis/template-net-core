using Dapper;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Customers;
using Template.Persistence.Services;
using Template.Persistence.Settings;

namespace Template.Persistence.RetrievalQueries
{
	internal class ExistingCustomersWithSameNameAndEmailRetrievalQuery : IQueryRetrievalPersistence<long, ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest, ExistingCustomersWithSameNameAndEmailRetrievalQueryResult>
	{
		private readonly IDbConnectionService _dbConnectionService;
		private readonly int _customerDatabaseTimeoutInSeconds;

		public ExistingCustomersWithSameNameAndEmailRetrievalQuery(
			IDbConnectionService dbConnectionService,
			DatabaseContextSettings databaseContextSettings)
		{
			_dbConnectionService = dbConnectionService;
			_customerDatabaseTimeoutInSeconds = databaseContextSettings.CustomerDatabaseTimeoutInSeconds;
		}

		public async Task<ExistingCustomersWithSameNameAndEmailRetrievalQueryResult> Retrieve(ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest request)
		{
			using var dbConnection = _dbConnectionService.CreateCustomerDatabaseSqlConnection();
			const string sql = "select * from Customers where Name = @name and Email = @email";
			var customers = await dbConnection.QueryAsync<Customer>(sql, new { Name = request.Name, Email = request.Email }, commandTimeout: _customerDatabaseTimeoutInSeconds);
			return ExistingCustomersWithSameNameAndEmailRetrievalQueryResult.Create(customers.Any());
		}
	}
}