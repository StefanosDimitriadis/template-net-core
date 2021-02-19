using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Services;

namespace Template.Api.HostedServices
{
	internal class ScheduleFirstOfMonthCustomerNotificationHostedService : IHostedService
	{
		private readonly IScheduler _scheduler;
		private readonly ILogger<ScheduleFirstOfMonthCustomerNotificationHostedService> _logger;

		public ScheduleFirstOfMonthCustomerNotificationHostedService(
			IScheduler scheduler,
			ILogger<ScheduleFirstOfMonthCustomerNotificationHostedService> logger)
		{
			_scheduler = scheduler;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("{hostedServiceName} started", nameof(ScheduleFirstOfMonthCustomerNotificationHostedService));
				await _scheduler.CreateNotifyPlayersOnFirstOfMonth();
				_logger.LogInformation("{hostedServiceName} completed", nameof(ScheduleFirstOfMonthCustomerNotificationHostedService));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "{hostedServiceName} could not run", nameof(ScheduleFirstOfMonthCustomerNotificationHostedService));
				throw;
			}
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("{hostedServiceName} started cleaning", nameof(ScheduleFirstOfMonthCustomerNotificationHostedService));
				await _scheduler.RemoveAll();
				_logger.LogInformation("{hostedServiceName} completed cleaning", nameof(ScheduleFirstOfMonthCustomerNotificationHostedService));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "{hostedServiceName} could not run", nameof(ScheduleFirstOfMonthCustomerNotificationHostedService));
				throw;
			}
		}
	}
}