using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Timer;

namespace Template.Api
{
	internal static class ApiMetricsRegistry
	{
		private const string Context = "Api";

		internal static TimerOptions MapTimer = new TimerOptions
		{
			Context = Context,
			Name = "Map Timer",
			MeasurementUnit = Unit.Items,
			DurationUnit = TimeUnit.Milliseconds,
			RateUnit = TimeUnit.Milliseconds
		};

		internal static TimerOptions HttpRequestHandlingTimer = new TimerOptions
		{
			Context = Context,
			Name = "Http Request Handling Timer",
			MeasurementUnit = Unit.Items,
			DurationUnit = TimeUnit.Milliseconds,
			RateUnit = TimeUnit.Milliseconds
		};

		internal static CounterOptions ValidationExceptionCounter = new CounterOptions
		{
			Context = Context,
			Name = "Validation Exception Counter",
			MeasurementUnit = Unit.Items
		};

		internal static CounterOptions ExceptionCounter = new CounterOptions
		{
			Context = Context,
			Name = "Exception Counter",
			MeasurementUnit = Unit.Items
		};

		internal static CounterOptions HttpRequestTotalCounter = new CounterOptions
		{
			Context = Context,
			Name = "Total Http Requests Counter",
			MeasurementUnit = Unit.Items
		};

		internal static CounterOptions HttpRequestCustomMiddlewareCounter = new CounterOptions
		{
			Context = Context,
			Name = "Custom Middleware Http Requests Counter",
			MeasurementUnit = Unit.Items
		};

		internal static CounterOptions HttpRequestSuccessfulHandlingCounter = new CounterOptions
		{
			Context = Context,
			Name = "Successfully Handled Http Requests Counter",
			MeasurementUnit = Unit.Items
		};

		internal static CounterOptions HttpRequestCustomMiddlewareSuccessfulHandlingCounter = new CounterOptions
		{
			Context = Context,
			Name = "Successfully Handled Custom Middleware Http Request Counter",
			MeasurementUnit = Unit.Items
		};

		internal static CounterOptions HttpRequestUnsuccessfulHandlingCounter = new CounterOptions
		{
			Context = Context,
			Name = "Unsuccessfully Handled Http Requests Counter",
			MeasurementUnit = Unit.Items
		};

		internal static CounterOptions HttpRequestCustomMiddlewareUnsuccessfulHandlingCounter = new CounterOptions
		{
			Context = Context,
			Name = "Unsuccessfully Handled Custom Middleware Http Requests Counter",
			MeasurementUnit = Unit.Items
		};
	}
}