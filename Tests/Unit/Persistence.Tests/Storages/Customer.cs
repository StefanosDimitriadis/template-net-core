using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Customers;
using Template.Persistence.DatabaseContexts;
using Xunit;

namespace Template.Persistence.Tests.Storages
{
	public class CustomerStorageUnitTests
	{
		[Fact]
		public async Task CustomerStorage_Should_Not_BeAbleToConnect()
		{
			var hostBuilder = CustomerStorageHostBuilderFactory.CreateWithWrongConnectionString();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			using var customerDatabaseContext = host.Services.GetRequiredService<CustomerDatabaseContext>();
			var canConnect = await customerDatabaseContext.Database.CanConnectAsync();
			canConnect.Should().BeFalse();
			await host.StopAsync();
		}

		[Fact]
		public async Task CustomerStorage_Should_BeAbleToConnect()
		{
			var hostBuilder = CustomerStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			using var customerDatabaseContext = host.Services.GetRequiredService<CustomerDatabaseContext>();
			var canConnect = await customerDatabaseContext.Database.CanConnectAsync();
			canConnect.Should().BeTrue();
			await host.StopAsync();
		}

		[Fact]
		public async Task CustomerStorage_Should_AddNewCustomer()
		{
			var hostBuilder = CustomerStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			await CreateCustomer(host);
			await Clear(host);
		}

		[Fact]
		public async Task CustomerStorage_Should_Not_GetNonExistingCustomer()
		{
			var hostBuilder = CustomerStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var customer = await CreateCustomer(host);
			await DeleteCustomer(host, customer);
			var nonExistingCustomer = await GetCustomer(host, customer.Id);
			nonExistingCustomer.Should().BeNull();
			await Clear(host);
		}

		[Fact]
		public async Task CustomerStorage_Should_GetExistingCustomer()
		{
			var hostBuilder = CustomerStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var customer = await CreateCustomer(host);
			var existingCustomer = await GetCustomer(host, customer.Id);
			existingCustomer.Should().NotBeNull();
			await Clear(host);
		}

		[Fact]
		public async Task CustomerStorage_Should_Not_UpdateNonExistingCustomer()
		{
			var hostBuilder = CustomerStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var customer = await CreateCustomer(host);
			await DeleteCustomer(host, customer);
			var nonExistingCustomer = await GetCustomer(host, customer.Id);
			nonExistingCustomer.Should().BeNull();
			await Clear(host);
		}

		[Fact]
		public async Task CustomerStorage_Should_UpdateExistingCustomer()
		{
			var hostBuilder = CustomerStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var customer = await CreateCustomer(host);
			var existingCustomer = await GetCustomer(host, customer.Id);
			var modification = existingCustomer.Update(email: "new email", dateOfBirth: DateTime.UtcNow);
			await UpdateCustomer(host, modification.Entity);
		}

		private async Task Clear(IHost host)
		{
			var customerQueryStorage = host.Services.GetRequiredService<ICustomerQueryStorage>();
			var customers = await customerQueryStorage.Get();
			if (customers.Length > 0)
			{
				using var customerDatabaseContext = host.Services.GetRequiredService<CustomerDatabaseContext>();
				customerDatabaseContext.RemoveRange(customers);
				await customerDatabaseContext.SaveChangesAsync();
			}
			await host.StopAsync();
			host.Dispose();
		}

		private async Task<Customer> CreateCustomer(IHost host)
		{
			var customer = CustomerFactory.Create(name: "name", email: "email", dateOfBirth: DateTime.UtcNow);
			var customerCommandStorage = host.Services.GetRequiredService<ICustomerCommandStorage>();
			customerCommandStorage.Create(customer);
			await customerCommandStorage.SaveChangesAsync();
			return customer;
		}

		private async Task DeleteCustomer(IHost host, Customer customer)
		{
			var customerDatabaseContext = host.Services.GetRequiredService<CustomerDatabaseContext>();
			customerDatabaseContext.Remove(customer);
			await customerDatabaseContext.SaveChangesAsync();
		}

		private async Task<Customer> GetCustomer(IHost host, long customerId)
		{
			var customerCommandStorage = host.Services.GetRequiredService<ICustomerQueryStorage>();
			return await customerCommandStorage.Get(customerId);
		}

		private async Task UpdateCustomer(IHost host, Customer customer)
		{
			var customerCommandStorage = host.Services.GetRequiredService<ICustomerCommandStorage>();
			customerCommandStorage.Update(customer);
			await customerCommandStorage.SaveChangesAsync();
		}
	}
}