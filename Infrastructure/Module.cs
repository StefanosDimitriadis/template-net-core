using Autofac;
using Template.Application.Services;
using Template.Infrastructure.Services;

namespace Template.Infrastructure
{
	public class InfrastructureModule : Module
	{
		protected override void Load(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterType<BonusDeletionNotifier>().As<IBonusDeletionNotifier>().SingleInstance();
			containerBuilder.RegisterType<CustomerEmailNotifier>().As<ICustomerNotifier>().SingleInstance();
			containerBuilder.RegisterType<MessageBroker>().As<IMessageBroker>().SingleInstance();
			containerBuilder.RegisterGeneric(typeof(DistributedCacheWrapper<,>)).AsImplementedInterfaces().SingleInstance();
			containerBuilder.RegisterType<Scheduler>().As<IScheduler>().SingleInstance();
			containerBuilder.RegisterType<PolicyExecutor>().As<IPolicyExecutor>().InstancePerDependency();
			containerBuilder.RegisterType<EmailMessageBuilder>().As<IEmailMessageBuilder>().SingleInstance();
		}
	}
}