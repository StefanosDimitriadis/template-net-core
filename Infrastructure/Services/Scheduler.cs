using Hangfire;
using System;
using System.Threading.Tasks;
using Template.Application.Services;

namespace Template.Infrastructure.Services
{
	internal class Scheduler : IScheduler
	{
		private readonly IRecurringJobManager _recurringJobManager;

		private const string NotifyPlayersOnFirstOfMonth = "NotifyPlayersOnFirstOfMonth";
		private static readonly string[] RecurringJobIds = new string[]
		{
			NotifyPlayersOnFirstOfMonth
		};
		private static readonly string[] BackgroundJobIds = Array.Empty<string>();

		public Scheduler(IRecurringJobManager recurringJobManager)
		{
			_recurringJobManager = recurringJobManager;
		}

		public Task CreateNotifyPlayersOnFirstOfMonth()
		{
			RecurringJob.AddOrUpdate<NotifyActiveCustomersOnFirstOfMonthJob>(
				recurringJobId: NotifyPlayersOnFirstOfMonth,
				methodCall: _job => _job.SendEmails(),
				cronExpression: Cron.Monthly());
			return Task.CompletedTask;
		}

		public Task RemoveAll()
		{
			try
			{
				foreach (var recurringJobId in RecurringJobIds)
					_recurringJobManager.RemoveIfExists(recurringJobId);
				foreach (var backgroundJobId in BackgroundJobIds)
					BackgroundJob.Delete(backgroundJobId);
				return Task.CompletedTask;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}