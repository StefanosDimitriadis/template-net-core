using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Application.Services;

namespace Template.Infrastructure.Services
{
	internal class PolicyExecutor : IPolicyExecutor
	{
		private readonly IList<IAsyncPolicy> _policies;
		private readonly ILogger<PolicyExecutor> _logger;

		public PolicyExecutor(ILogger<PolicyExecutor> logger)
		{
			_policies = new List<IAsyncPolicy>();
			_logger = logger;
		}

		public async Task Retry<TException>(
			Func<Task> onRetry,
			int retryCount,
			Func<Task> onFailure = null,
			IDictionary<string, object> contextData = null) where TException : Exception
		{
			Func<Context, Task<object>> innerOnRetry = async (_) =>
			{
				await onRetry();
				return Task.FromResult(new object());
			};
			await Retry<object, TException>(innerOnRetry, retryCount, onFailure, contextData);
		}

		private async Task<TResult> Retry<TResult, TException>(
			Func<Context, Task<TResult>> onRetry,
			int retryCount,
			Func<Task> onFailure = null,
			IDictionary<string, object> contextData = null) where TException : Exception
		{
			AddRetryPolicy<TException>(retryCount);
			return await GetResult(onRetry, onFailure, contextData);
		}

		private void AddRetryPolicy<TException>(int retryCount) where TException : Exception
		{
			var retryPolicy = Policy
				.Handle<TException>()
				.RetryAsync(
					retryCount,
					(_exception, _retryAttempt, _context) =>
					{
						_logger.LogError(_exception, $"CorrelationId: [{_context.CorrelationId}] - {_context.PolicyKey} at {_context.OperationKey}: {_retryAttempt} attempt");
					});
			_policies.Add(retryPolicy);
		}

		private async Task<TResult> GetResult<TResult>(Func<Context, Task<TResult>> onRetry, Func<Task> onFailure = null, IDictionary<string, object> contextData = null)
		{
			if (onFailure != null)
			{
				var policyResult = await GetResult(onRetry, contextData);
				if (policyResult == null || policyResult.Outcome == OutcomeType.Failure)
				{
					await onFailure();
					return default;
				}
				else
				{
					return policyResult.Result ?? default;
				}
			}
			else
			{
				return await ExecuteAsync(onRetry, contextData);
			}
		}

		private async Task<PolicyResult<TResult>> GetResult<TResult>(Func<Context, Task<TResult>> onRetry, IDictionary<string, object> contextData = null)
		{
			if (_policies.Count == 1)
			{
				return await _policies[0].ExecuteAndCaptureAsync(() =>
				{
					return onRetry(null);
				});
			}

			var policyWrap = Policy.WrapAsync(_policies.ToArray());
			var policyResult = await policyWrap.ExecuteAndCaptureAsync(
				_context =>
				{
					return onRetry(_context);
				},
				contextData);
			_policies.Clear();
			return policyResult;
		}

		private async Task<TResult> ExecuteAsync<TResult>(Func<Context, Task<TResult>> onRetry, IDictionary<string, object> contextData = null)
		{
			if (_policies.Count == 1)
			{
				return await _policies[0].ExecuteAsync(() =>
				{
					return onRetry(null);
				});
			}

			var policyWrap = Policy.WrapAsync(_policies.ToArray());
			var result = await policyWrap.ExecuteAsync(
				_context =>
				{
					return onRetry(_context);
				},
				contextData);
			_policies.Clear();
			return result;
		}
	}
}