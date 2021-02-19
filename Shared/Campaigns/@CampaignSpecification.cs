namespace Template.Shared.Campaigns
{
	public class CampaignSpecification
	{
		public decimal BonusMoney { get; }

		public CampaignSpecification(decimal bonusMoney)
		{
			BonusMoney = bonusMoney;
		}
	}
}