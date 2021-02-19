using System;
using System.Threading.Tasks;
using Template.Application.Persistence;
using Template.Domain.Entities;
using Template.Domain.Entities.Customers;

namespace Template.Application.Commands.Customers
{
	internal class DeleteCustomerMetricDecorator : IEntityModificationPersistence<long, long, Customer, DeleteModification<long, long, Customer>>
	{
		private readonly IEntityModificationPersistence<long, long, Customer, DeleteModification<long, long, Customer>> _entityModificationPersistence;
		private readonly IMetrics _metrics;

		public DeleteCustomerMetricDecorator(
			IEntityModificationPersistence<long, long, Customer, DeleteModification<long, long, Customer>> entityModificationPersistence,
			IMetrics metrics)
		{
			_entityModificationPersistence = entityModificationPersistence;
			_metrics = metrics;
		}

		public async Task Persist(DeleteModification<long, long, Customer> modification)
		{
			try
			{
				await _entityModificationPersistence.Persist(modification);
				_metrics.DecreaseCounter(ApplicationMetricsRegistry.ActiveCustomersCounter);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}