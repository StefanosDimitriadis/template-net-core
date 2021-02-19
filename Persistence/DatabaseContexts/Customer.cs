using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Template.Domain.Entities.Customers;

namespace Template.Persistence.DatabaseContexts
{
	public class CustomerDatabaseContext : DbContext
	{
		public DbSet<Customer> Customers { get; set; }

		public CustomerDatabaseContext(DbContextOptions<CustomerDatabaseContext> dbContextOptions) : base(dbContextOptions) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			modelBuilder.ApplyConfigurationsFromAssembly(executingAssembly, _type => _type.Name.Contains(nameof(Customer)));
		}
	}
}