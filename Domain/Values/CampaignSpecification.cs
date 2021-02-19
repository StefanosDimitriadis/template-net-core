using System;

namespace Template.Domain.Values
{
	public class CampaignSpecification
	{
		public decimal BonusMoney { get; }

		private CampaignSpecification() { }

		public CampaignSpecification(decimal bonusMoney)
		{
			BonusMoney = bonusMoney;
		}

		public override bool Equals(object obj)
		{
			return obj != null
				&& obj is CampaignSpecification specification
				&& BonusMoney == specification.BonusMoney;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(BonusMoney);
		}
	}
}