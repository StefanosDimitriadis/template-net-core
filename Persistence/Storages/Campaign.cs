using Dapper;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Campaigns;
using Template.Persistence.DatabaseContexts;
using Template.Persistence.Services;
using Template.Persistence.Settings;

namespace Template.Persistence.Storages
{
	internal class CampaignCommandStorage : ICampaignCommandStorage
	{
		private readonly CampaignDatabaseContext _campaignDatabaseContext;

		public CampaignCommandStorage(CampaignDatabaseContext campaignDatabaseContext)
		{
			_campaignDatabaseContext = campaignDatabaseContext;
		}

		public void Create(Campaign campaign)
		{
			_campaignDatabaseContext.Campaigns.Add(campaign);
		}

		public void Create(Campaign[] campaigns)
		{
			_campaignDatabaseContext.Campaigns.AddRange(campaigns);
		}

		public async Task SaveChangesAsync()
		{
			await _campaignDatabaseContext.SaveChangesAsync();
		}

		public void Update(Campaign campaign)
		{
			_campaignDatabaseContext.Campaigns.Update(campaign);
			//Use this approach to achieve better performance since it updates only the selected properties
			//_customerDatabaseContext.Attach(campaign);
			//_campaignDatabaseContext.Entry(campaign).Property(_campaign => _campaign.IsDeleted).IsModified = true;
		}
	}

	internal class CampaignQueryStorage : ICampaignQueryStorage
	{
		private readonly ISqlConnectionService _sqlConnectionService;
		private readonly int _campaignDatabaseTimeoutInSeconds;

		public CampaignQueryStorage(
			ISqlConnectionService sqlConnectionService,
			DatabaseContextSettings databaseContextSettings)
		{
			_sqlConnectionService = sqlConnectionService;
			_campaignDatabaseTimeoutInSeconds = databaseContextSettings.CampaignDatabaseTimeoutInSeconds;
		}

		public async Task<Campaign> Get(long id)
		{
			using var dbConnection = _sqlConnectionService.CreateCampaignDatabaseSqlConnection();
			const string sql = "select * from Campaigns where Id = @id";
			return (await dbConnection.QueryAsync(sql, new { id = id }, commandTimeout: _campaignDatabaseTimeoutInSeconds))
				.Select(_object =>
				{
					return Campaign.Create(_object.Id, _object.BonusMoney, _object.CreatedAt, _object.UpdatedAt, _object.IsDeleted);
				})
				.Cast<Campaign>()
				.FirstOrDefault();
		}

		public async Task<Campaign[]> Get()
		{
			using var dbConnection = _sqlConnectionService.CreateCampaignDatabaseSqlConnection();
			const string sql = "select * from Campaigns";
			return (await dbConnection.QueryAsync<Campaign>(sql, commandTimeout: _campaignDatabaseTimeoutInSeconds)).ToArray();
		}
	}
}