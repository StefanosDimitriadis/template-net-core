using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Template.Domain.Entities.Campaigns;

namespace Template.Persistence.DatabaseContexts
{
	public class CampaignDatabaseContext : DbContext
	{
		public DbSet<Campaign> Campaigns { get; set; }

		public CampaignDatabaseContext(DbContextOptions<CampaignDatabaseContext> dbContextOptions) : base(dbContextOptions) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			modelBuilder.ApplyConfigurationsFromAssembly(executingAssembly, _type => _type.Name.Contains(nameof(Campaign)));
		}
	}
}