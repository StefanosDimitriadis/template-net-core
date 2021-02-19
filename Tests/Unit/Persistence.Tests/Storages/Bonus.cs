using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Bonuses;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Entities.Customers;
using Template.Domain.Values;
using Template.Persistence.DatabaseContexts;
using Xunit;

namespace Template.Persistence.Tests.Storages
{
	public class BonusStorageUnitTests
	{
		[Fact]
		public async Task BonusStorage_Should_Not_BeAbleToConnect()
		{
			var hostBuilder = BonusStorageHostBuilderFactory.CreateWithWrongConnectionString();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			using var bonusDatabaseContext = host.Services.GetRequiredService<BonusDatabaseContext>();
			var canConnect = await bonusDatabaseContext.Database.CanConnectAsync();
			canConnect.Should().BeFalse();
			await host.StopAsync();
		}

		[Fact]
		public async Task BonusStorage_Should_BeAbleToConnect()
		{
			var hostBuilder = BonusStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			using var bonusDatabaseContext = host.Services.GetRequiredService<BonusDatabaseContext>();
			var canConnect = await bonusDatabaseContext.Database.CanConnectAsync();
			canConnect.Should().BeTrue();
			await host.StopAsync();
		}

		[Fact]
		public async Task BonusStorage_Should_Not_AddNewBonusWithNonExistingCampaignOrCustomer()
		{
			var hostBuilder = BonusStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			try
			{
				await CreateBonusWithNonExistingCampaignOrCustomer(host);
			}
			catch (DbUpdateException exception)
			{
				exception.Entries.Should().HaveCountGreaterThan(0);
				exception.InnerException.Should().NotBeNull();
			}
			finally
			{
				await Clear(host);
			}
		}

		[Fact]
		public async Task BonusStorage_Should_AddNewBonusWithExistingCampaignAndCustomer()
		{
			var hostBuilder = BonusStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaignId = await CreateCampaign(host);
			var customerId = await CreateCustomer(host);
			await CreateBonusWithExistingCampaignAndCustomer(host, campaignId, customerId);
			await Clear(host);
		}

		[Fact]
		public async Task BonusStorage_Should_Not_GetNonExistingBonus()
		{
			var hostBuilder = BonusStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaignId = await CreateCampaign(host);
			var customerId = await CreateCustomer(host);
			var bonus = await CreateBonusWithExistingCampaignAndCustomer(host, campaignId, customerId);
			await DeleteBonus(host, bonus);
			var nonExistingBonus = await GetBonus(host, bonus.Id);
			nonExistingBonus.Should().BeNull();
			await Clear(host);
		}

		[Fact]
		public async Task BonusStorage_Should_GetExistingBonus()
		{
			var hostBuilder = BonusStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaignId = await CreateCampaign(host);
			var customerId = await CreateCustomer(host);
			var bonus = await CreateBonusWithExistingCampaignAndCustomer(host, campaignId, customerId);
			var existingBonus = await GetBonus(host, bonus.Id);
			existingBonus.Should().NotBeNull();
			await Clear(host);
		}

		[Fact]
		public async Task BonusStorage_Should_Not_UpdateNonExistingBonus()
		{
			var hostBuilder = BonusStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaignId = await CreateCampaign(host);
			var customerId = await CreateCustomer(host);
			var bonus = await CreateBonusWithExistingCampaignAndCustomer(host, campaignId, customerId);
			await DeleteBonus(host, bonus);
			var nonExistingBonus = await GetBonus(host, bonus.Id);
			nonExistingBonus.Should().BeNull();
			await Clear(host);
		}

		[Fact]
		public async Task BonusStorage_Should_UpdateExistingBonus()
		{
			var hostBuilder = BonusStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaignId = await CreateCampaign(host);
			var customerId = await CreateCustomer(host);
			var bonus = await CreateBonusWithExistingCampaignAndCustomer(host, campaignId, customerId);
			var existingBonus = await GetBonus(host, bonus.Id);
			var modification = existingBonus.Award();
			await UpdateBonus(host, modification.Entity);
		}

		private async Task Clear(IHost host)
		{
			var bonusQueryStorage = host.Services.GetRequiredService<IBonusQueryStorage>();
			var bonuses = await bonusQueryStorage.Get();
			if (bonuses.Length > 0)
			{
				using var bonusDatabaseContext = host.Services.GetRequiredService<BonusDatabaseContext>();
				bonusDatabaseContext.RemoveRange(bonuses);
				await bonusDatabaseContext.SaveChangesAsync();
			}
			var campaignQueryStorage = host.Services.GetRequiredService<ICampaignQueryStorage>();
			var campaigns = await campaignQueryStorage.Get();
			if (campaigns.Length > 0)
			{
				using var campaignDatabaseContext = host.Services.GetRequiredService<CampaignDatabaseContext>();
				campaignDatabaseContext.RemoveRange(campaigns);
				await campaignDatabaseContext.SaveChangesAsync();
			}
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

		private async Task CreateBonusWithNonExistingCampaignOrCustomer(IHost host)
		{
			var bonusCommandStorage = host.Services.GetRequiredService<IBonusCommandStorage>();
			var bonus = BonusFactory.CreateWithNonExistingCampaignOrCustomer();
			bonusCommandStorage.Create(bonus);
			await bonusCommandStorage.SaveChangesAsync();
		}

		private async Task<Bonus> CreateBonusWithExistingCampaignAndCustomer(IHost host, long campaignId, long customerId)
		{
			var bonus = BonusFactory.CreateWithExistingCampaignAndCustomer(campaignId, customerId);
			var bonusCommandStorage = host.Services.GetRequiredService<IBonusCommandStorage>();
			bonusCommandStorage.Create(bonus);
			await bonusCommandStorage.SaveChangesAsync();
			return bonus;
		}

		private async Task<long> CreateCustomer(IHost host)
		{
			var customerCommandStorage = host.Services.GetRequiredService<ICustomerCommandStorage>();
			var customer = Customer.Create("test customer", "test email", DateTime.UtcNow).Entity;
			customerCommandStorage.Create(customer);
			await customerCommandStorage.SaveChangesAsync();
			return customer.Id;
		}

		private async Task<long> CreateCampaign(IHost host)
		{
			var campaignCommandStorage = host.Services.GetRequiredService<ICampaignCommandStorage>();
			var campaign = Campaign.Create(new CampaignSpecification(1)).Entity;
			campaignCommandStorage.Create(campaign);
			await campaignCommandStorage.SaveChangesAsync();
			return campaign.Id;
		}

		private async Task DeleteBonus(IHost host, Bonus bonus)
		{
			var bonusDatabaseContext = host.Services.GetRequiredService<BonusDatabaseContext>();
			bonusDatabaseContext.Remove(bonus);
			await bonusDatabaseContext.SaveChangesAsync();
		}

		private async Task<Bonus> GetBonus(IHost host, long bonusId)
		{
			var bonusQueryStorage = host.Services.GetRequiredService<IBonusQueryStorage>();
			return await bonusQueryStorage.Get(bonusId);
		}

		private async Task UpdateBonus(IHost host, Bonus bonus)
		{
			var bonusCommandStorage = host.Services.GetRequiredService<IBonusCommandStorage>();
			bonusCommandStorage.Update(bonus);
			await bonusCommandStorage.SaveChangesAsync();
		}
	}
}