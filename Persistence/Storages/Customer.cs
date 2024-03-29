﻿using Dapper;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Customers;
using Template.Persistence.DatabaseContexts;
using Template.Persistence.Services;
using Template.Persistence.Settings;

[assembly: InternalsVisibleTo("Template.Persistence.Tests")]
namespace Template.Persistence.Storages
{
	internal class CustomerCommandStorage : ICustomerCommandStorage
	{
		private readonly CustomerDatabaseContext _customerDatabaseContext;

		public CustomerCommandStorage(CustomerDatabaseContext customerDatabaseContext)
		{
			_customerDatabaseContext = customerDatabaseContext;
		}

		public void Create(Customer customer)
		{
			_customerDatabaseContext.Customers.Add(customer);
		}

		public void Create(Customer[] customers)
		{
			_customerDatabaseContext.Customers.AddRange(customers);
		}

		public async Task SaveChangesAsync()
		{
			await _customerDatabaseContext.SaveChangesAsync();
		}

		public void Update(Customer customer)
		{
			_customerDatabaseContext.Customers.Update(customer);
			//Use this approach to achieve better performance since it updates only the selected properties
			//_customerDatabaseContext.Attach(customer);
			//_customerDatabaseContext.Entry(customer).Property(_customer => _customer.IsDeleted).IsModified = true;
		}
	}

	internal class CustomerQueryStorage : ICustomerQueryStorage
	{
		private readonly IDbConnectionService _dbConnectionService;
		private readonly int _customerDatabaseTimeoutInSeconds;

		public CustomerQueryStorage(
			IDbConnectionService dbConnectionService,
			DatabaseContextSettings databaseContextSettings)
		{
			_dbConnectionService = dbConnectionService;
			_customerDatabaseTimeoutInSeconds = databaseContextSettings.CustomerDatabaseTimeoutInSeconds;
		}

		public async Task<Customer> Get(long id)
		{
			using var dbConnection = _dbConnectionService.CreateCustomerDatabaseSqlConnection();
			const string sql = "select * from Customers where Id = @id";
			return await dbConnection.QuerySingleOrDefaultAsync<Customer>(sql, new { id = id }, commandTimeout: _customerDatabaseTimeoutInSeconds);
		}

		public async Task<Customer[]> Get()
		{
			using var dbConnection = _dbConnectionService.CreateCustomerDatabaseSqlConnection();
			const string sql = "select * from Customers";
			return (await dbConnection.QueryAsync<Customer>(sql, commandTimeout: _customerDatabaseTimeoutInSeconds)).ToArray();
		}
	}
}