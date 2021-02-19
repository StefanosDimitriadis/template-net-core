namespace Template.Application.RetrievalQueries
{
	public abstract class BaseRequest<TId>
	{
		public abstract TId Id { get; protected set; }
	}

	public abstract class BaseResult<TId>
	{
		public abstract TId Id { get; protected set; }
	}
}