using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Customers;

namespace Template.Persistence.DatabaseContexts
{
	internal class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> entityTypeBuilder)
		{
			entityTypeBuilder.ToTable($"{nameof(Customer)}s");
			entityTypeBuilder.HasKey(_entity => _entity.Id);
			entityTypeBuilder.Property(_entity => _entity.Name).HasMaxLength(50).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.Email).HasMaxLength(50).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.DateOfBirth).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.Money).HasColumnType("money").IsRequired();
			entityTypeBuilder.Property(_entity => _entity.CreatedAt).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.UpdatedAt);
			entityTypeBuilder.Property(_entity => _entity.IsDeleted).IsRequired();
		}
	}
}