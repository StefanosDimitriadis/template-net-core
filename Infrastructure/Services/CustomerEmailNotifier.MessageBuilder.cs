using MimeKit;
using MimeKit.Text;
using Template.Application.Services;
using Template.Domain.Entities.Customers;
using Template.Domain.Values;

namespace Template.Infrastructure.Services
{
	internal interface IEmailMessageBuilder
	{
		MimeMessage CreateRegistrationMessage(Customer customer);
		MimeMessage CreateNewBonusMessage(Customer customer, CampaignSpecification campaignSpecification);
		MimeMessage CreateAwardMessage(Customer customer, decimal moneyToAdd);
		MimeMessage CreateFirstOfMonthMessage(Customer customer);
	}

	internal class EmailMessageBuilder : IEmailMessageBuilder
	{
		private readonly CustomerNotifierEmailSettings _customerNotifierEmailSettings;

		public EmailMessageBuilder(CustomerNotifierEmailSettings customerNotifierEmailSettings)
		{
			_customerNotifierEmailSettings = customerNotifierEmailSettings;
		}

		public MimeMessage CreateRegistrationMessage(Customer customer)
		{
			var message = new MimeMessage
			{
				Subject = _customerNotifierEmailSettings.RegistrationMessageSubject,
				Body = new TextPart(TextFormat.Plain)
				{
					Text = GetRegistrationEmailMessageText(customer.Name)
				}
			};
			message.From.Add(new MailboxAddress(_customerNotifierEmailSettings.SenderName, _customerNotifierEmailSettings.SenderEmailAddress));
			message.To.Add(new MailboxAddress(customer.Name, customer.Email));
			return message;
		}

		public MimeMessage CreateNewBonusMessage(Customer customer, CampaignSpecification campaignSpecification)
		{
			var message = new MimeMessage
			{
				Subject = _customerNotifierEmailSettings.NewBonusMessageSubject,
				Body = new TextPart(TextFormat.Plain)
				{
					Text = GetNewBonusEmailMessageText(customer.Name, campaignSpecification.BonusMoney)
				}
			};
			message.From.Add(new MailboxAddress(_customerNotifierEmailSettings.SenderName, _customerNotifierEmailSettings.SenderEmailAddress));
			message.To.Add(new MailboxAddress(customer.Name, customer.Email));
			return message;
		}

		public MimeMessage CreateAwardMessage(Customer customer, decimal moneyToAdd)
		{
			var message = new MimeMessage
			{
				Subject = _customerNotifierEmailSettings.AwardMessageSubject,
				Body = new TextPart(TextFormat.Plain)
				{
					Text = GetAwardEmailMessageText(customer.Name, moneyToAdd)
				}
			};
			message.From.Add(new MailboxAddress(_customerNotifierEmailSettings.SenderName, _customerNotifierEmailSettings.SenderEmailAddress));
			message.To.Add(new MailboxAddress(customer.Name, customer.Email));
			return message;
		}

		public MimeMessage CreateFirstOfMonthMessage(Customer customer)
		{
			var message = new MimeMessage
			{
				Subject = _customerNotifierEmailSettings.FirstOfMonthMessageSubject,
				Body = new TextPart(TextFormat.Plain)
				{
					Text = GetFirstOfMonthEmailMessageText(customer.Name)
				}
			};
			message.From.Add(new MailboxAddress(_customerNotifierEmailSettings.SenderName, _customerNotifierEmailSettings.SenderEmailAddress));
			message.To.Add(new MailboxAddress(customer.Name, customer.Email));
			return message;
		}

		private string GetNewBonusEmailMessageText(string customerName, decimal bonusMoney)
		{
			return string.Format(_customerNotifierEmailSettings.NewBonusMessageText, customerName, bonusMoney);
		}

		private string GetAwardEmailMessageText(string customerName, decimal moneyToAdd)
		{
			return string.Format(_customerNotifierEmailSettings.AwardMessageText, customerName, moneyToAdd);
		}

		private string GetRegistrationEmailMessageText(string customerName)
		{
			return string.Format(_customerNotifierEmailSettings.RegistrationMessageText, customerName);
		}

		private string GetFirstOfMonthEmailMessageText(string customerName)
		{
			return string.Format(_customerNotifierEmailSettings.FirstOfMonthMessageText, customerName);
		}
	}
}