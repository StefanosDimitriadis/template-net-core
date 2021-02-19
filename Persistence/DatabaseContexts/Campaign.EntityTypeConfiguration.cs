using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Campaigns;

namespace Template.Persistence.DatabaseContexts
{
	internal class CampaignEntityTypeConfiguration : IEntityTypeConfiguration<Campaign>
	{
		public void Configure(EntityTypeBuilder<Campaign> entityTypeBuilder)
		{
			entityTypeBuilder.ToTable($"{nameof(Campaign)}s");
			entityTypeBuilder.HasKey(_entity => _entity.Id);
			entityTypeBuilder.OwnsOne(
				_entity => _entity.CampaignSpecification,
				_ownedNavigationBuilder =>
				{
					_ownedNavigationBuilder.Property(_ownedEntity => _ownedEntity.BonusMoney)
						.HasColumnType("money")
						.HasColumnName("BonusMoney")
						.IsRequired();
				});
			entityTypeBuilder.Property(_entity => _entity.CreatedAt).IsRequired();
			entityTypeBuilder.Property(_entity => _entity.UpdatedAt);
			entityTypeBuilder.Property(_entity => _entity.IsDeleted).IsRequired();
		}
	}
}