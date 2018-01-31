
namespace PhoneChoiceHelper.Modules
{
    using Autofac;
    using Dne.Core.Logging;
    using Dne.Web.Http.Configuration;

    public sealed class AutofacLoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NLogger>().As<ILogger>();
        }
    }
}
