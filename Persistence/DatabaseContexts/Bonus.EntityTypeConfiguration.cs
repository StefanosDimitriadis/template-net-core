using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Bonuses;
using Template.Domain.Entities.Campaigns;
using Template.Domain.Entities.Customers;

namespace Template.Persistence.DatabaseContexts
{
	internal class BonusEntityTypeConfiguration : IEntityTypeConfiguration<Bonus>
	{
		public void Configure(EntityTypeBuilder<Bonus> entityTypeBuilder)
		{
			entityTypeBuilder.ToTable($"{nameof(Bonus)}es");
			entityTypeBuilder.HasKey(_entity => _entity.Id);
			entityTypeBuilder.HasOne<Campaign>().WithOne().HasForeignKey<Campaign>(_campaign => _campaign.Id);
			entityTypeBuilder.HasOne<Customer>().WithMany().HasForeignKey(_entity => _entity.CustomerId);
			entityTypeBuilder.Property(_entity => _entity.CampaignId).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.CustomerId).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.HasBeenAwarded).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.CreatedAt).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.UpdatedAt);
			entityTypeBuilder.Property(_entity => _entity.IsDeleted).IsRequired();
		}
	}
}