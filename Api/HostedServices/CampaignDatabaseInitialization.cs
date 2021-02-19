using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Values;

namespace Template.Api.HostedServices
{
	internal class CampaignDatabaseInitializationHostedService : IHostedService
	{
		private readonly ICampaignCommandStorage _campaignCommandStorage;
		private readonly ICampaignQueryStorage _campaignQueryStorage;
		private readonly ILogger<CampaignDatabaseInitializationHostedService> _logger;

		public CampaignDatabaseInitializationHostedService(
			ICampaignCommandStorage campaignCommandStorage,
			ICampaignQueryStorage campaignQueryStorage,
			ILogger<CampaignDatabaseInitializationHostedService> logger)
		{
			_campaignCommandStorage = campaignCommandStorage;
			_campaignQueryStorage = campaignQueryStorage;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("{hostedServiceName} started", nameof(CampaignDatabaseInitializationHostedService));
				var databaseHasBeenInitialized = (await _campaignQueryStorage.Get()).Length > 0;
				if (!databaseHasBeenInitialized)
				{
					var initialCampaigns = CreateInitialCampaigns();
					_campaignCommandStorage.Create(initialCampaigns);
					await _campaignCommandStorage.SaveChangesAsync();
					_logger.LogInformation("{hostedServiceName} added initial campaigns", nameof(CampaignDatabaseInitializationHostedService));
				}
				_logger.LogInformation("{hostedServiceName} completed", nameof(CampaignDatabaseInitializationHostedService));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "{hostedServiceName} could not run", nameof(CampaignDatabaseInitializationHostedService));
				throw;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private Campaign[] CreateInitialCampaigns()
		{
			return new Campaign[]
			{
				Campaign.Create(new CampaignSpecification(10)).Entity,
				Campaign.Create(new CampaignSpecification(20)).Entity,
				Campaign.Create(new CampaignSpecification(30)).Entity,
				Campaign.Create(new CampaignSpecification(40)).Entity,
				Campaign.Create(new CampaignSpecification(50)).Entity
			};
		}
	}
}