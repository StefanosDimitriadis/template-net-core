using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Application.Services
{
	public interface IPolicyExecutor
	{
		Task Retry<TException>(Func<Task> onRetry, int retryCount, Func<Task> onFailure = null, IDictionary<string, object> contextData = null) where TException : Exception;
	}
}