using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Application.Services;
using Template.Domain.Entities.Customers;
using Template.Domain.Values;

namespace Template.Infrastructure.Services
{
	internal class CustomerEmailNotifier : ICustomerNotifier
	{
		private readonly IPolicyExecutor _policyExecutor;
		private readonly CustomerNotifierEmailSettings _customerNotifierEmailSettings;
		private readonly PolicyExecutorSettings _policyExecutorSettings;
		private readonly IEmailMessageBuilder _emailMessageBuilder;
		private readonly ILogger<CustomerEmailNotifier> _logger;

		public CustomerEmailNotifier(
			IPolicyExecutor policyExecutor,
			CustomerNotifierEmailSettings customerNotifierEmailSettings,
			PolicyExecutorSettings policyExecutorSettings,
			IEmailMessageBuilder emailMessageBuilder,
			ILogger<CustomerEmailNotifier> logger)
		{
			_policyExecutor = policyExecutor;
			_customerNotifierEmailSettings = customerNotifierEmailSettings;
			_policyExecutorSettings = policyExecutorSettings;
			_emailMessageBuilder = emailMessageBuilder;
			_logger = logger;
		}

		public async Task NotifyForAward(Customer customer, decimal moneyToAdd)
		{
			try
			{
				await SendEmailForAward(customer, moneyToAdd);
			}
			catch (Exception exception)
			{
				await _policyExecutor.Retry<Exception>(
					async () =>
					{
						await SendEmailForAward(customer, moneyToAdd);
					},
					_policyExecutorSettings.NotifyCustomerRetryCount,
					() =>
					{
						_logger.LogError(exception, $"Email notification for bonus award failed - Customer id: [{customer.Id}]");
						return Task.CompletedTask;
					},
					new Dictionary<string, object>());
			}
		}

		public async Task NotifyForNewBonus(Customer customer, CampaignSpecification campaignSpecification)
		{
			try
			{
				await SendEmailForNewBonus(customer, campaignSpecification);
			}
			catch (Exception exception)
			{
				await _policyExecutor.Retry<Exception>(
					async () =>
					{
						await SendEmailForNewBonus(customer, campaignSpecification);
					},
					_policyExecutorSettings.NotifyCustomerRetryCount,
					() =>
					{
						_logger.LogError(exception, $"Email notification for new bonus failed - Customer id: [{customer.Id}]");
						return Task.CompletedTask;
					},
					new Dictionary<string, object>());
			}
		}

		public async Task NotifyForRegistration(Customer customer)
		{
			try
			{
				await SendEmailForRegistration(customer);
			}
			catch (Exception exception)
			{
				await _policyExecutor.Retry<Exception>(
					async () =>
					{
						await SendEmailForRegistration(customer);
					},
					_policyExecutorSettings.NotifyCustomerRetryCount,
					() =>
					{
						_logger.LogError(exception, $"Email notification for registration failed - Customer name: [{customer.Name}] and customer email: [{customer.Email}]");
						return Task.CompletedTask;
					},
					new Dictionary<string, object>());
			}
		}

		public async Task NotifyOnFirstOfMonth(Customer customer)
		{
			var message = _emailMessageBuilder.CreateFirstOfMonthMessage(customer);
			await SendMessage(message);
		}

		private async Task SendEmailForAward(Customer customer, decimal moneyToAdd)
		{
			var message = _emailMessageBuilder.CreateAwardMessage(customer, moneyToAdd);
			await SendMessage(message);
		}

		private async Task SendEmailForNewBonus(Customer customer, CampaignSpecification campaignSpecification)
		{
			var message = _emailMessageBuilder.CreateNewBonusMessage(customer, campaignSpecification);
			await SendMessage(message);
		}

		private async Task SendEmailForRegistration(Customer customer)
		{
			var message = _emailMessageBuilder.CreateRegistrationMessage(customer);
			await SendMessage(message);
		}

		private async Task SendMessage(MimeMessage message)
		{
			using var smtpClient = new SmtpClient();
			await smtpClient.ConnectAsync(_customerNotifierEmailSettings.SmtpHost, _customerNotifierEmailSettings.SmtpPort, _customerNotifierEmailSettings.SmtpUseSsl);
			await smtpClient.AuthenticateAsync(_customerNotifierEmailSettings.Username, _customerNotifierEmailSettings.Password);
			await smtpClient.SendAsync(message);
			await smtpClient.DisconnectAsync(quit: true);
		}
	}
}