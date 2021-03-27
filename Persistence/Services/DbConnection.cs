using Microsoft.Data.SqlClient;
using System.Data;
using Template.Persistence.Settings;

namespace Template.Persistence.Services
{
	internal interface IDbConnectionService
	{
		IDbConnection CreateBonusDatabaseSqlConnection();
		IDbConnection CreateCampaignDatabaseSqlConnection();
		IDbConnection CreateCustomerDatabaseSqlConnection();
	}

	internal class DbConnectionService : IDbConnectionService
	{
		private readonly DatabaseContextSettings _databaseContextSettings;

		public DbConnectionService(DatabaseContextSettings databaseContextSettings)
		{
			_databaseContextSettings = databaseContextSettings;
		}

		public IDbConnection CreateBonusDatabaseSqlConnection()
		{
			return new SqlConnection(_databaseContextSettings.BonusDatabaseConnectionString);
		}

		public IDbConnection CreateCampaignDatabaseSqlConnection()
		{
			return new SqlConnection(_databaseContextSettings.CampaignDatabaseConnectionString);
		}

		public IDbConnection CreateCustomerDatabaseSqlConnection()
		{
			return new SqlConnection(_databaseContextSettings.CustomerDatabaseConnectionString);
		}
	}
}