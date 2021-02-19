using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Values;
using Template.Persistence.DatabaseContexts;
using Xunit;

namespace Template.Persistence.Tests.Storages
{
	public class CampaignStorageUnitTests
	{
		[Fact]
		public async Task CampaignStorage_Should_Not_BeAbleToConnect()
		{
			var hostBuilder = CampaignStorageHostBuilderFactory.CreateWithWrongConnectionString();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			using var campaignDatabaseContext = host.Services.GetRequiredService<CampaignDatabaseContext>();
			var canConnect = await campaignDatabaseContext.Database.CanConnectAsync();
			canConnect.Should().BeFalse();
			await host.StopAsync();
		}

		[Fact]
		public async Task CampaignStorage_Should_BeAbleToConnect()
		{
			var hostBuilder = CampaignStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			using var campaignDatabaseContext = host.Services.GetRequiredService<CampaignDatabaseContext>();
			var canConnect = await campaignDatabaseContext.Database.CanConnectAsync();
			canConnect.Should().BeTrue();
			await host.StopAsync();
		}

		[Fact]
		public async Task CampaignStorage_Should_AddNewCampaign()
		{
			var hostBuilder = CampaignStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			await CreateCampaign(host);
			await Clear(host);
		}

		[Fact]
		public async Task CampaignStorage_Should_Not_GetNonExistingCampaign()
		{
			var hostBuilder = CampaignStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaign = await CreateCampaign(host);
			await DeleteCampaign(host, campaign);
			var nonExistingCampaign = await GetCampaign(host, campaign.Id);
			nonExistingCampaign.Should().BeNull();
			await Clear(host);
		}

		[Fact]
		public async Task CampaignStorage_Should_GetExistingCampaign()
		{
			var hostBuilder = CampaignStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaign = await CreateCampaign(host);
			var existingCampaign = await GetCampaign(host, campaign.Id);
			existingCampaign.Should().NotBeNull();
			await Clear(host);
		}

		[Fact]
		public async Task CampaignStorage_Should_Not_UpdateNonExistingCampaign()
		{
			var hostBuilder = CampaignStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaign = await CreateCampaign(host);
			await DeleteCampaign(host, campaign);
			var nonExistingCampaign = await GetCampaign(host, campaign.Id);
			nonExistingCampaign.Should().BeNull();
			await Clear(host);
		}

		[Fact]
		public async Task CampaignStorage_Should_UpdateExistingCampaign()
		{
			var hostBuilder = CampaignStorageHostBuilderFactory.Create();
			using var host = hostBuilder.Build();
			await host.StartAsync();
			var campaign = await CreateCampaign(host);
			var existingCampaign = await GetCampaign(host, campaign.Id);
			var modification = existingCampaign.Update(new CampaignSpecification(bonusMoney: 20));
			await UpdateCampaign(host, modification.Entity);
		}

		private async Task Clear(IHost host)
		{
			var campaignQueryStorage = host.Services.GetRequiredService<ICampaignQueryStorage>();
			var campaigns = await campaignQueryStorage.Get();
			if (campaigns.Length > 0)
			{
				using var campaignDatabaseContext = host.Services.GetRequiredService<CampaignDatabaseContext>();
				campaignDatabaseContext.RemoveRange(campaigns);
				await campaignDatabaseContext.SaveChangesAsync();
			}
			await host.StopAsync();
			host.Dispose();
		}

		private async Task<Campaign> CreateCampaign(IHost host)
		{
			var campaign = CampaignFactory.Create(bonusMoney: 10);
			var campaignCommandStorage = host.Services.GetRequiredService<ICampaignCommandStorage>();
			campaignCommandStorage.Create(campaign);
			await campaignCommandStorage.SaveChangesAsync();
			return campaign;
		}

		private async Task DeleteCampaign(IHost host, Campaign campaign)
		{
			var campaignDatabaseContext = host.Services.GetRequiredService<CampaignDatabaseContext>();
			campaignDatabaseContext.Remove(campaign);
			await campaignDatabaseContext.SaveChangesAsync();
		}

		private async Task<Campaign> GetCampaign(IHost host, long campaignId)
		{
			var campaignQueryStorage = host.Services.GetRequiredService<ICampaignQueryStorage>();
			return await campaignQueryStorage.Get(campaignId);
		}

		private async Task UpdateCampaign(IHost host, Campaign campaign)
		{
			var campaignCommandStorage = host.Services.GetRequiredService<ICampaignCommandStorage>();
			campaignCommandStorage.Update(campaign);
			await campaignCommandStorage.SaveChangesAsync();
		}
	}
}