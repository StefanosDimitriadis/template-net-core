namespace Template.Application.Services
{
	public abstract class BaseMessageBrokerMessage { }

	public class BonusDeletionMessage : BaseMessageBrokerMessage
	{
		public long BonusId { get; }

		public BonusDeletionMessage(long bonusId)
		{
			BonusId = bonusId;
		}
	}
}