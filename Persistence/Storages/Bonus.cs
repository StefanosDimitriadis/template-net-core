using Dapper;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.Persistence.Storages;
using Template.Domain.Entities.Bonuses;
using Template.Persistence.DatabaseContexts;
using Template.Persistence.Services;
using Template.Persistence.Settings;

namespace Template.Persistence.Storages
{
	internal class BonusCommandStorage : IBonusCommandStorage
	{
		private readonly BonusDatabaseContext _bonusDatabaseContext;

		public BonusCommandStorage(BonusDatabaseContext bonusDatabaseContext)
		{
			_bonusDatabaseContext = bonusDatabaseContext;
		}

		public void Create(Bonus bonus)
		{
			_bonusDatabaseContext.Bonuses.Add(bonus);
		}

		public async Task SaveChangesAsync()
		{
			await _bonusDatabaseContext.SaveChangesAsync();
		}

		public void Update(Bonus bonus)
		{
			_bonusDatabaseContext.Bonuses.Update(bonus);
			//Use this approach to achieve better performance since it updates only the selected properties
			//_bonusDatabaseContext.Attach(bonus);
			//_bonusDatabaseContext.Entry(bonus).Property(_bonus => _bonus.IsDeleted).IsModified = true;
		}
	}

	internal class BonusQueryStorage : IBonusQueryStorage
	{
		private readonly ISqlConnectionService _sqlConnectionService;
		private readonly int _bonusDatabaseTimeoutInSeconds;

		public BonusQueryStorage(
			ISqlConnectionService sqlConnectionService,
			DatabaseContextSettings databaseContextSettings)
		{
			_sqlConnectionService = sqlConnectionService;
			_bonusDatabaseTimeoutInSeconds = databaseContextSettings.BonusDatabaseTimeoutInSeconds;
		}

		public async Task<Bonus> Get(long id)
		{
			using var dbConnection = _sqlConnectionService.CreateBonusDatabaseSqlConnection();
			const string sql = "select * from Bonuses where Id = @id";
			return await dbConnection.QuerySingleOrDefaultAsync<Bonus>(sql, new { id = id }, commandTimeout: _bonusDatabaseTimeoutInSeconds);
		}

		public async Task<Bonus[]> Get()
		{
			using var dbConnection = _sqlConnectionService.CreateBonusDatabaseSqlConnection();
			const string sql = "select * from Bonuses";
			return (await dbConnection.QueryAsync<Bonus>(sql, commandTimeout: _bonusDatabaseTimeoutInSeconds)).ToArray();
		}
	}
}