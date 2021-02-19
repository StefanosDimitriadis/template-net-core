using System;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Customers;

namespace Template.Application.Commands.Customers
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
				_metrics.IncreaseCounter(ApplicationMetricsRegistry.CreatedCustomersCounter);
				_metrics.IncreaseCounter(ApplicationMetricsRegistry.ActiveCustomersCounter);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}