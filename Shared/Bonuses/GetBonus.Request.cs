namespace Template.Shared.Bonuses
{
	public class GetBonusRequest
	{
		public long Id { get; }

		public GetBonusRequest(long id)
		{
			Id = id;
		}
	}
}