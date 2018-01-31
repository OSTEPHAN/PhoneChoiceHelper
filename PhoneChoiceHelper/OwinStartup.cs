
[assembly: Microsoft.Owin.OwinStartup(typeof(PhoneChoiceHelper.OwinStartup))]
namespace PhoneChoiceHelper
{
    using Dne.Core.Configuration;
    using Dne.Web.Http;
    using Dne.Web.Http.Configuration;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    using Owin;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Swashbuckle.Application;

    public class OwinStartup
    {
        protected readonly IDependencyResolver dependencyResolver = new AutofacDependencyResolver();

        public void Configuration(IAppBuilder app)
        {
            dependencyResolver.Register(this.GetType().Assembly);

            var oAuthAuthorizationServerProvider = new OAuthAuthorizationServerProvider()
            {
                OnValidateClientAuthentication = async c => await this.ValidateClientAuthentication(c),
                OnGrantResourceOwnerCredentials = async c => await this.GrantResourceOwnerCredentials(c),
            };

            var oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(this.GetTokenEndpointPath()),
                Provider = oAuthAuthorizationServerProvider,
                AllowInsecureHttp = true,
            };

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            //app.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);
            app.UseConfiguration(dependencyResolver);

            dependencyResolver.Resolve<HttpConfiguration>()
                .EnableSwagger(c => c.SingleApiVersion("v1", "PhoneChoiceHelper API"))
                .EnableSwaggerUi();
        }
        protected virtual string GetTokenEndpointPath()
        {
            return "/oauth/token";
        }

        protected virtual Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext oAuthValidateClientAuthenticationContext)
        {
            oAuthValidateClientAuthenticationContext.Validated();
            return Task.FromResult<object>(null);
        }

        protected virtual Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext oAuthGrantResourceOwnerCredentialsContext)
        {
            var userName = oAuthGrantResourceOwnerCredentialsContext.UserName;
            var userPassword = oAuthGrantResourceOwnerCredentialsContext.Password;

            oAuthGrantResourceOwnerCredentialsContext.Rejected();
            return Task.FromResult<object>(null);
        }
    }
}

