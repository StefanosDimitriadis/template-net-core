//using App.Metrics;
//using App.Metrics.Counter;
//using App.Metrics.Timer;

//namespace Template.Application
//{
//	internal static class ApplicationMetricsRegistry
//	{
//		private const string Context = "Application";

//		internal static TimerOptions MessageBrokerPublishTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Message Broker Publish Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static TimerOptions MessageBrokerConsumerTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Message Broker Consumer Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static TimerOptions QueryHandlerTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Query Handler Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static TimerOptions CommandHandlerTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Command Handler Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static TimerOptions NotificationHandlerTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Notification Handler Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static TimerOptions CommandPersistenceTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Command Persistence Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static TimerOptions QueryRetrievalPersistenceTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Query Retrieval Persistence Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static TimerOptions DistributedCacheWrapperAddTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Distributed Cache Wrapper Add Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static TimerOptions DistributedCacheWrapperGetTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Distributed Cache Wrapper Get Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		internal static CounterOptions AwardedBonusesCounter = new CounterOptions
//		{
//			Context = Context,
//			Name = "Awarded Bonuses Counter",
//			MeasurementUnit = Unit.Items
//		};

//		internal static CounterOptions CreatedBonusesCounter = new CounterOptions
//		{
//			Context = Context,
//			Name = "Created Bonuses Counter",
//			MeasurementUnit = Unit.Items
//		};

//		internal static CounterOptions CreatedCampaignsCounter = new CounterOptions
//		{
//			Context = Context,
//			Name = "Created Campaigns Counter",
//			MeasurementUnit = Unit.Items
//		};

//		internal static CounterOptions CreatedCustomersCounter = new CounterOptions
//		{
//			Context = Context,
//			Name = "Created Customers Counter",
//			MeasurementUnit = Unit.Items
//		};

//		internal static CounterOptions ActiveCustomersCounter = new CounterOptions
//		{
//			Context = Context,
//			Name = "Active Customers Counter",
//			MeasurementUnit = Unit.Items
//		};

//		internal static CounterOptions AwardedBonusAmountCounter = new CounterOptions
//		{
//			Context = Context,
//			Name = "Awarded Bonus Amount Counter",
//			MeasurementUnit = Unit.Items
//		};

//		internal static TimerOptions EntityRetrievalPersistenceTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Entity Retrieval Persistence Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};

//		public static TimerOptions EntityModificationPersistenceTimer = new TimerOptions
//		{
//			Context = Context,
//			Name = "Entity Modification Persistence Timer",
//			MeasurementUnit = Unit.Items,
//			DurationUnit = TimeUnit.Milliseconds,
//			RateUnit = TimeUnit.Milliseconds
//		};
//	}
//}