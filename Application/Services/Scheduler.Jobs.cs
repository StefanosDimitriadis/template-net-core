using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Persistence.RetrievalQueries;

namespace Template.Application.Services
{
	public abstract class BaseJob { }

	public class NotifyActiveCustomersOnFirstOfMonthJob : BaseJob
	{
		private readonly IQueryRetrievalPersistence<long, ActiveCustomersRetrievalQueryResult> _queryRetrievalPersistence;
		private readonly ICustomerNotifier _customerNotifier;
		private readonly ILogger<NotifyActiveCustomersOnFirstOfMonthJob> _logger;

		public NotifyActiveCustomersOnFirstOfMonthJob(
			IQueryRetrievalPersistence<long, ActiveCustomersRetrievalQueryResult> queryRetrievalPersistence,
			ICustomerNotifier customerNotifier,
			ILogger<NotifyActiveCustomersOnFirstOfMonthJob> logger)
		{
			_queryRetrievalPersistence = queryRetrievalPersistence;
			_customerNotifier = customerNotifier;
			_logger = logger;
		}

		public async Task SendEmails()
		{
			try
			{
				_logger.LogInformation("{jobName} started", nameof(SendEmails));
				var retrievalQueryResult = await _queryRetrievalPersistence.Retrieve();
				foreach (var activeCustomer in retrievalQueryResult.ActiveCustomers)
				{
					try
					{
						await _customerNotifier.NotifyOnFirstOfMonth(activeCustomer);
					}
					catch (Exception exception)
					{
						_logger.LogError(exception, "{task} failed for customer with id: [{customerId}]", nameof(ICustomerNotifier.NotifyOnFirstOfMonth), activeCustomer.Id);
					}
				}
				_logger.LogInformation("{jobName} completed", nameof(SendEmails));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "{jobName} failed", nameof(SendEmails));
			}
		}
	}
}