
namespace PhoneChoiceHelper.Controllers
{
    using Dne.Core.Logging;
    using Dne.Core.Storage;
    using Swashbuckle.Swagger.Annotations;
    using System.ComponentModel;
    using System.Net;
    using System.Web.Http;

    [DisplayName("Installation")]
    [RoutePrefix("api/install")]
    public class InstallController : ApiController
    {
        private readonly ILogger logger;
        private readonly IEntityStore entityStore;
        public InstallController(ILogger logger, IEntityStore entityStore)
        {
            this.logger = logger;
            this.entityStore = entityStore; ;
        }

        [HttpPost]
        [Route]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public IHttpActionResult Post()
        {
            this.entityStore.Install(string.Empty, string.Empty);
            return Ok();
        }
    }
}