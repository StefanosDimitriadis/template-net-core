namespace Template.Shared.Bonuses
{
	public class DeleteBonusRequest
	{
		public long Id { get; }

		public DeleteBonusRequest(long id)
		{
			Id = id;
		}
	}
}