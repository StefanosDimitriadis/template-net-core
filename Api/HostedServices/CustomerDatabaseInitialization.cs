using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Customers;

namespace Template.Api.HostedServices
{
	internal class CustomerDatabaseInitializationHostedService : IHostedService
	{
		private readonly ICustomerCommandStorage _customerCommandStorage;
		private readonly ICustomerQueryStorage _customerQueryStorage;
		private readonly ILogger<CustomerDatabaseInitializationHostedService> _logger;

		public CustomerDatabaseInitializationHostedService(
			ICustomerCommandStorage customerCommandStorage,
			ICustomerQueryStorage customerQueryStorage,
			ILogger<CustomerDatabaseInitializationHostedService> logger)
		{
			_customerCommandStorage = customerCommandStorage;
			_customerQueryStorage = customerQueryStorage;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("{hostedServiceName} started", nameof(CustomerDatabaseInitializationHostedService));
				var databaseHasBeenInitialized = (await _customerQueryStorage.Get()).Length > 0;
				if (!databaseHasBeenInitialized)
				{
					var initialCustomers = CreateInitialCustomers();
					_customerCommandStorage.Create(initialCustomers);
					await _customerCommandStorage.SaveChangesAsync();
					_logger.LogInformation("{hostedServiceName} added initial customers", nameof(CustomerDatabaseInitializationHostedService));
				}
				_logger.LogInformation("{hostedServiceName} completed", nameof(CustomerDatabaseInitializationHostedService));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "{hostedServiceName} could not run", nameof(CustomerDatabaseInitializationHostedService));
				throw;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private Customer[] CreateInitialCustomers()
		{
			return new Customer[]
			{
				Customer.Create("first customer", "first email", DateTime.UtcNow.AddMinutes(1)).Entity,
				Customer.Create("second customer", "second email", DateTime.UtcNow.AddMinutes(2)).Entity,
				Customer.Create("third customer", "third email", DateTime.UtcNow.AddMinutes(3)).Entity,
				Customer.Create("fourth customer", "fourth email", DateTime.UtcNow.AddMinutes(4)).Entity,
				Customer.Create("fifth customer", "fifth email", DateTime.UtcNow.AddMinutes(5)).Entity
			};
		}
	}
}