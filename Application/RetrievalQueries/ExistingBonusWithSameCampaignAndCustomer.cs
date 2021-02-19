using Template.Application.RetrievalQueries;
using Template.Domain.Services;

namespace Template.Persistence.RetrievalQueries
{
	public class ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest : BaseRequest<long>
	{
		public override long Id { get; protected set; }
		public long CampaignId { get; }
		public long CustomerId { get; }

		private ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest(long id, long campaignId, long customerId)
		{
			Id = id;
			CampaignId = campaignId;
			CustomerId = customerId;
		}

		public static ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest Create(long campaignId, long customerId)
		{
			return new ExistingBonusWithSameCampaignAndCustomerRetrievalQueryRequest(IdGenerator.Generate(), campaignId, customerId);
		}
	}

	public class ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult : BaseResult<long>
	{
		public override long Id { get; protected set; }
		public bool ExistingBonusWithSameCampaignAndCustomer { get; }

		private ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult(long id, bool existingBonusWithSameCampaignAndCustomer)
		{
			Id = id;
			ExistingBonusWithSameCampaignAndCustomer = existingBonusWithSameCampaignAndCustomer;
		}

		public static ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult Create(bool existingBonusWithSameCampaignAndCustomer)
		{
			return new ExistingBonusWithSameCampaignAndCustomerRetrievalQueryResult(IdGenerator.Generate(), existingBonusWithSameCampaignAndCustomer);
		}
	}
}