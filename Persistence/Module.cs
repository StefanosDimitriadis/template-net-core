using Autofac;
using Template.Application.Persistence;
using Template.Application.Persistence.Storages;
using Template.Persistence.Storages;

namespace Template.Persistence
{
	public class PersistenceModule : Module
	{
		protected override void Load(ContainerBuilder containerBuilder)
		{
			RegisterPersistence(containerBuilder);
			RegisterStorage(containerBuilder);
		}

		private void RegisterPersistence(ContainerBuilder containerBuilder)
		{
			var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			containerBuilder.RegisterAssemblyTypes(executingAssembly).AsClosedTypesOf(typeof(IEntityModificationPersistence<,,,>));
			containerBuilder.RegisterAssemblyTypes(executingAssembly).AsClosedTypesOf(typeof(IEntityRetrievalPersistence<,>));
			containerBuilder.RegisterAssemblyTypes(executingAssembly).AsClosedTypesOf(typeof(ICommandPersistence<,>));
			containerBuilder.RegisterAssemblyTypes(executingAssembly).AsClosedTypesOf(typeof(IQueryRetrievalPersistence<,,>));
			containerBuilder.RegisterAssemblyTypes(executingAssembly).AsClosedTypesOf(typeof(IQueryRetrievalPersistence<,>));
		}

		private void RegisterStorage(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterType<BonusCommandStorage>().As<IBonusCommandStorage>().InstancePerLifetimeScope();
			containerBuilder.RegisterType<BonusQueryStorage>().As<IBonusQueryStorage>().SingleInstance();
			containerBuilder.RegisterType<CampaignCommandStorage>().As<ICampaignCommandStorage>().InstancePerLifetimeScope();
			containerBuilder.RegisterType<CampaignQueryStorage>().As<ICampaignQueryStorage>().SingleInstance();
			containerBuilder.RegisterType<CustomerCommandStorage>().As<ICustomerCommandStorage>().InstancePerLifetimeScope();
			containerBuilder.RegisterType<CustomerQueryStorage>().As<ICustomerQueryStorage>().SingleInstance();
			containerBuilder.RegisterType<EventCommandStorage<long, long>>().As<IEventCommandStorage<long, long>>().SingleInstance();
		}
	}
}