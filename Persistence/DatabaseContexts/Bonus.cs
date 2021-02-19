using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Template.Domain.Entities.Bonuses;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Entities.Customers;

namespace Template.Persistence.DatabaseContexts
{
	public class BonusDatabaseContext : DbContext
	{
		public DbSet<Bonus> Bonuses { get; set; }

		public BonusDatabaseContext(DbContextOptions<BonusDatabaseContext> dbContextOptions) : base(dbContextOptions) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			modelBuilder.ApplyConfigurationsFromAssembly(executingAssembly, _type => _type.Name.Contains(nameof(Bonus)));
			modelBuilder.ApplyConfigurationsFromAssembly(executingAssembly, _type => _type.Name.Contains(nameof(Campaign)));
			modelBuilder.ApplyConfigurationsFromAssembly(executingAssembly, _type => _type.Name.Contains(nameof(Customer)));
		}
	}
}