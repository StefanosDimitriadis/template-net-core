using Dapper;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities.Customers;
using Template.Persistence.Settings;

namespace Template.Persistence.RetrievalQueries
{
	internal class ExistingCustomersWithSameNameAndEmailRetrievalQuery : IQueryRetrievalPersistence<long, ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest, ExistingCustomersWithSameNameAndEmailRetrievalQueryResult>
	{
		private readonly string _customerDatabaseConnectionString;

		public ExistingCustomersWithSameNameAndEmailRetrievalQuery(DatabaseContextSettings databaseContextSettings)
		{
			_customerDatabaseConnectionString = databaseContextSettings.CustomerDatabaseConnectionString;
		}

		public async Task<ExistingCustomersWithSameNameAndEmailRetrievalQueryResult> Retrieve(ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest request)
		{
			using var dbConnection = new SqlConnection(_customerDatabaseConnectionString);
			const string sql = "select * from Customers where Name = @name and Email = @email";
			var customers = await dbConnection.QueryAsync<Customer>(sql, new { Name = request.Name, Email = request.Email });
			return ExistingCustomersWithSameNameAndEmailRetrievalQueryResult.Create(customers.Any());
		}
	}
}