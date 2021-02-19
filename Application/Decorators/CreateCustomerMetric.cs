using App.Metrics;
using System;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Customers;

namespace Template.Application.Decorators
{
	internal class CreateCustomerMetricDecorator : IEntityModificationPersistence<long, long, Customer, CreateModification<long, long, Customer>>
	{
		private readonly IEntityModificationPersistence<long, long, Customer, CreateModification<long, long, Customer>> _entityModificationPersistence;
		private readonly IMetrics _metrics;

		public CreateCustomerMetricDecorator(
			IEntityModificationPersistence<long, long, Customer, CreateModification<long, long, Customer>> entityModificationPersistence,
			IMetrics metrics)
		{
			_entityModificationPersistence = entityModificationPersistence;
			_metrics = metrics;
		}

		public async Task Persist(CreateModification<long, long, Customer> modification)
		{
			try
			{
				await _entityModificationPersistence.Persist(modification);
				_metrics.Measure.Counter.Increment(ApplicationMetricsRegistry.CreatedCustomersCounter);
				_metrics.Measure.Counter.Increment(ApplicationMetricsRegistry.ActiveCustomersCounter);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}