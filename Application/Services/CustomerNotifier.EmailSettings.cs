using System;
using System.Net.Sockets;

namespace Template.Application.Services
{
	public class CustomerNotifierEmailSettings : IValidatable
	{
		public const string SettingsName = nameof(CustomerNotifierEmailSettings);

		public string Username { get; set; }
		public string Password { get; set; }
		public int TimeoutInMilliseconds { get; set; }
		public string SmtpHost { get; set; }
		public int SmtpPort { get; set; }
		public bool SmtpUseSsl { get; set; }
		public string SenderName { get; set; }
		public string SenderEmailAddress { get; set; }
		public string NewBonusMessageSubject { get; set; }
		public string NewBonusMessageText { get; set; }
		public string AwardMessageSubject { get; set; }
		public string AwardMessageText { get; set; }
		public string RegistrationMessageSubject { get; set; }
		public string RegistrationMessageText { get; set; }
		public string FirstOfMonthMessageSubject { get; set; }
		public string FirstOfMonthMessageText { get; set; }

		public void Validate()
		{
			if (string.IsNullOrWhiteSpace(Username))
				throw new ArgumentException(message: "Username cannot be null", paramName: nameof(Username));

			if (string.IsNullOrWhiteSpace(Password))
				throw new ArgumentException(message: "Password cannot be null", paramName: nameof(Password));

			if (TimeoutInMilliseconds < 1)
				throw new ArgumentException(message: "Timeout should be greater than 0", paramName: nameof(TimeoutInMilliseconds));

			try
			{
				var tcpClient = new TcpClient();
				tcpClient.Connect(SmtpHost, SmtpPort);
			}
			catch (Exception exception)
			{
				throw new ArgumentException(message: "Use valid host and port for smtp", paramName: $"{nameof(SmtpHost)}-{nameof(SmtpPort)}", exception);
			}

			if (string.IsNullOrWhiteSpace(SenderName))
				throw new ArgumentException(message: "Sender name cannot be null", paramName: nameof(SenderName));

			if (string.IsNullOrWhiteSpace(SenderEmailAddress))
				throw new ArgumentException(message: "Sender email address cannot be null", paramName: nameof(SenderEmailAddress));

			if (string.IsNullOrWhiteSpace(NewBonusMessageSubject))
				throw new ArgumentException(message: "Subject for new bonus message cannot be null", paramName: nameof(NewBonusMessageSubject));

			if (string.IsNullOrWhiteSpace(NewBonusMessageText))
				throw new ArgumentException(message: "Text for new bonus message cannot be null", paramName: nameof(NewBonusMessageText));

			if (string.IsNullOrWhiteSpace(AwardMessageSubject))
				throw new ArgumentException(message: "Subject for award message cannot be null", paramName: nameof(AwardMessageSubject));

			if (string.IsNullOrWhiteSpace(AwardMessageText))
				throw new ArgumentException(message: "Text for award message cannot be null", paramName: nameof(AwardMessageText));

			if (string.IsNullOrWhiteSpace(RegistrationMessageSubject))
				throw new ArgumentException(message: "Subject for registration message cannot be null", paramName: nameof(RegistrationMessageSubject));

			if (string.IsNullOrWhiteSpace(RegistrationMessageText))
				throw new ArgumentException(message: "Text for registration message cannot be null", paramName: nameof(RegistrationMessageText));

			if (string.IsNullOrWhiteSpace(FirstOfMonthMessageSubject))
				throw new ArgumentException(message: "Subject for first of month message cannot be null", paramName: nameof(FirstOfMonthMessageSubject));

			if (string.IsNullOrWhiteSpace(FirstOfMonthMessageText))
				throw new ArgumentException(message: "Text for first of month message cannot be null", paramName: nameof(FirstOfMonthMessageText));
		}
	}
}