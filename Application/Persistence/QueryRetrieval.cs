using System.Threading.Tasks;
using Template.Application.RetrievalQueries;

namespace Template.Application.Persistence
{
	public interface IQueryRetrievalPersistence<TId, TRequest, TResult>
		where TRequest : BaseRequest<TId>
		where TResult : BaseResult<TId>
	{
		Task<TResult> Retrieve(TRequest request);
	}

	public interface IQueryRetrievalPersistence<TId, TResult>
		where TResult : BaseResult<TId>
	{
		Task<TResult> Retrieve();
	}
}