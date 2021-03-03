using Microsoft.Data.SqlClient;
using Template.Persistence.Settings;

namespace Template.Persistence.Services
{
	internal interface ISqlConnectionService
	{
		SqlConnection CreateBonusDatabaseSqlConnection();
		SqlConnection CreateCampaignDatabaseSqlConnection();
		SqlConnection CreateCustomerDatabaseSqlConnection();
	}

	internal class SqlConnectionService : ISqlConnectionService
	{
		private readonly DatabaseContextSettings _databaseContextSettings;

		public SqlConnectionService(DatabaseContextSettings databaseContextSettings)
		{
			_databaseContextSettings = databaseContextSettings;
		}

		public SqlConnection CreateBonusDatabaseSqlConnection()
		{
			return new SqlConnection(_databaseContextSettings.BonusDatabaseConnectionString);
		}

		public SqlConnection CreateCampaignDatabaseSqlConnection()
		{
			return new SqlConnection(_databaseContextSettings.CampaignDatabaseConnectionString);
		}

		public SqlConnection CreateCustomerDatabaseSqlConnection()
		{
			return new SqlConnection(_databaseContextSettings.CustomerDatabaseConnectionString);
		}
	}
}