
namespace PhoneChoiceHelper.Modules
{
    using Autofac;
    using Dne.Core.Logging;
    using Dne.Core.Storage;
    using Dne.Storage.EntityFramework;
    using Migrations;
    using Model;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    internal class AutofacStorageModule : Module
    {
        internal class DummyContext : MemoryContext
        {
            private readonly ILogger logger;
            private readonly static Dictionary<Type, IEnumerable> Repository = new Dictionary<Type, IEnumerable>() { };

            public DummyContext(ILogger logger) : base(DummyContext.Repository)
            {
                this.logger = logger;
            }

            public override void Install(string plateform, string environment)
            {
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MigrationsContextFactory>().As<IDataContextFactory>().As<IDbContextFactory<DataContext>>();
            builder.Register<DbContext>(ctx => ctx.Resolve<IDataContextFactory>().Create());
            builder.RegisterType<DummyContext>().SingleInstance().As<IEntityStore>();
            builder.Register<IEntityQuery>(ctx => ctx.Resolve<IEntityStore>());
        }
    }
}
