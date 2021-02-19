using System;

namespace Template.Application.Services
{
	public class PolicyExecutorSettings : IValidatable
	{
		public const string SettingsName = nameof(PolicyExecutorSettings);

		public ushort NotifyCustomerRetryCount { get; set; }

		public void Validate()
		{
			if (NotifyCustomerRetryCount < 1)
				throw new ArgumentException(message: "Notify customer retry count cannot be negative", paramName: nameof(NotifyCustomerRetryCount));
		}
	}
}