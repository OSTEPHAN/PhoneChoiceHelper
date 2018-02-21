
namespace UnitTest.PhoneChoiceHelper
{
    using Autofac;
    using Dne.Core.Configuration;
    using Dne.Core.Logging;
    using Dne.Core.Storage;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class AutofacLoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmptyLogger>().As<ILogger>();
        }
    }

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
            builder.RegisterType<DummyContext>().SingleInstance().As<IEntityStore>();
            builder.Register<IEntityQuery>(ctx => ctx.Resolve<IEntityStore>());
        }
    }

    internal class AutofacDependencyResolver : IDependencyResolver
    {
        private static IDependencyResolver _singleton;
        public static IDependencyResolver Singleton
        {
            get
            {
                if (null == _singleton)
                {
                    _singleton = new AutofacDependencyResolver() as IDependencyResolver;
                    _singleton.Register(typeof(AutofacDependencyResolver).Assembly);
                    _singleton.Register<global::PhoneChoiceHelper.Controllers.ApiShopItemController>();
                    _singleton.Build();
                }
                return _singleton;
            }
        }
        
        private IContainer container;
        private ContainerBuilder containerBuilder;
        private ContainerBuilder ContainerBuilder
        {
            get
            {
                if (null == this.containerBuilder)
                {
                    this.containerBuilder = new ContainerBuilder();
                }
                return this.containerBuilder;
            }
        }

        void IDependencyResolver.Build()
        {
            this.container = this.ContainerBuilder.Build();
        }

        void IDependencyResolver.Register(System.Reflection.Assembly assembly)
        {
            var registrars = assembly
                .GetTypes()
                .Where(t => false == t.IsAbstract && t.BaseType == typeof(Module))
                .Select(m => Activator.CreateInstance(m))
                .Cast<Module>()
                .Select(m => this.ContainerBuilder.RegisterModule(m))
                .ToList();
        }

        void IDependencyResolver.Register<TImplementation>()
        {
            this.ContainerBuilder.RegisterType<TImplementation>();
        }

        void IDependencyResolver.Register<TInterface, TImplementation>()
        {
            this.ContainerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        void IDependencyResolver.Register<T>(Func<T> factory)
        {
            this.ContainerBuilder.Register<T>(ctx => factory());
        }

        void IDependencyResolver.Register<T>(T instance)
        {
            var registerInstanceMethod = typeof(RegistrationExtensions).GetMethods().FirstOrDefault(m => m.Name == "RegisterInstance");
            registerInstanceMethod = registerInstanceMethod.MakeGenericMethod(new Type[] { typeof(T) });
            registerInstanceMethod.Invoke(null, new object[] { this.ContainerBuilder, instance });
        }

        T IDependencyResolver.Resolve<T>()
        {
            return this.container.Resolve<T>();
        }
    }
}
