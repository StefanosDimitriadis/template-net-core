using Dapper;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Customers;
using Template.Persistence.Settings;

namespace Template.Persistence.RetrievalQueries
{
	internal class ActiveCustomersRetrievalQuery : IQueryRetrievalPersistence<long, ActiveCustomersRetrievalQueryResult>
	{
		private readonly string _customerDatabaseConnectionString;

		public ActiveCustomersRetrievalQuery(DatabaseContextSettings databaseContextSettings)
		{
			_customerDatabaseConnectionString = databaseContextSettings.CustomerDatabaseConnectionString;
		}

		public async Task<ActiveCustomersRetrievalQueryResult> Retrieve()
		{
			using var dbConnection = new SqlConnection(_customerDatabaseConnectionString);
			const string sql = "select * from Customers where IsDeleted = @IsDeleted";
			var activeCustomers = await dbConnection.QueryAsync<Customer>(sql, new { IsDeleted = false });
			return ActiveCustomersRetrievalQueryResult.Create(new ReadOnlyCollection<Customer>(activeCustomers.AsList()));
		}
	}
}