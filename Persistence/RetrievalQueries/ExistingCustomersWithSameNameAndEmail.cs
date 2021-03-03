﻿using Dapper;
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
		private readonly ISqlConnectionService _sqlConnectionService;
		private readonly int _customerDatabaseTimeoutInSeconds;

		public ExistingCustomersWithSameNameAndEmailRetrievalQuery(
			ISqlConnectionService sqlConnectionService,
			DatabaseContextSettings databaseContextSettings)
		{
			_sqlConnectionService = sqlConnectionService;
			_customerDatabaseTimeoutInSeconds = databaseContextSettings.CustomerDatabaseTimeoutInSeconds;
		}

		public async Task<ExistingCustomersWithSameNameAndEmailRetrievalQueryResult> Retrieve(ExistingCustomersWithSameNameAndEmailRetrievalQueryRequest request)
		{
			using var dbConnection = _sqlConnectionService.CreateCustomerDatabaseSqlConnection();
			const string sql = "select * from Customers where Name = @name and Email = @email";
			var customers = await dbConnection.QueryAsync<Customer>(sql, new { Name = request.Name, Email = request.Email }, commandTimeout: _customerDatabaseTimeoutInSeconds);
			return ExistingCustomersWithSameNameAndEmailRetrievalQueryResult.Create(customers.Any());
		}
	}
}