namespace Contact.Infrastructure.AutofacModules
{
    using Autofac;
    using Contact.Commands;
    using Contact.Domain;
    using Contact.Projection.Query;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core.Query;
    using LeadsPlus.Core.Repositories;
    using Microsoft.Extensions.Configuration;
    using System.Reflection;

    public class ApplicationModule : Autofac.Module
    {
        public IConfiguration setting;

        public ApplicationModule(IConfiguration setting)
        {
            this.setting = setting;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ContactCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

            builder.RegisterType<QueryExecutor>().As<IQueryExecutor>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(GetContactQueryHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>)).AsImplementedInterfaces(); ;

            builder.RegisterType<QueryExecutor>()
                .As<IQueryExecutor>()
                .InstancePerLifetimeScope();

            builder.Register(c => { return ViewModelStoreFactory.Create<Contact>(setting["DatabaseConnectionString"], setting["DatabaseName"]); }).SingleInstance();

            //builder.Register(c => { return ViewModelStoreFactory.Create<Contact>(this.configuration["ProjectionDatabaseConnectionString"], "ContactProjections"); }).SingleInstance();

            //for view models
            //builder.Register(c => { return ViewModelStoreFactory.Create<ContactViewModel>(this.configuration["ProjectionDatabaseConnectionString"], "ContactProjections"); }).SingleInstance();

        }
    }
}
