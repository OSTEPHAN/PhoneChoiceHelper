
namespace PhoneChoiceHelper.Controllers
{
    using Dne.Core.Logging;
    using Dne.Core.Storage;
    using Swashbuckle.Swagger.Annotations;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;
    public class ShopController : System.Web.Mvc.Controller
    {
        private readonly ILogger logger;
        public ShopController(ILogger logger)
        {
            this.logger = logger;
        }

        public System.Web.Mvc.ActionResult Item(string id)
        {
            var shopItem = new Model.ShopItem() { Id = System.Guid.Parse(id), };
            return View(shopItem);
        }
    }

    [DisplayName("ShopItem matching")]
    [RoutePrefix("api/shopitem")]
    public class ApiShopItemController : ApiController
    {
        private readonly ILogger logger;
        private readonly IEntityStore entityStore;
        public ApiShopItemController(ILogger logger, IEntityStore entityStore)
        {
            this.logger = logger;
            this.entityStore = entityStore; ;
        }

        [PhoneChoiceHelper.Queryable]
        [HttpGet]
        [Route]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public IQueryable<Model.ShopItem> Get()
        {
            return this.entityStore.Query<Model.ShopItem>();
        }

        const string subscriptionKey = "601fa9cf0e3f41c18ed94dfe70909a38";
        const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/analyze";

        [HttpPost]
        [Route]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Post(Model.ShopItem shopItem)
        {
            var model = this.entityStore.Create<Model.ShopItem>();
            model.Id = System.Guid.NewGuid();

            var regex = System.Text.RegularExpressions.Regex.Match(
                shopItem.SerializedImage,
                @"data:(?<type>.+?);base64,(?<data>.+)");
            var base64Data =  regex.Groups["data"].Value;
            var byteData = System.Convert.FromBase64String(base64Data);

            var request = new HttpClient();
            request.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string requestParameters = "visualFeatures=Categories,Tags,Description";
            string uri = uriBase + "?" + requestParameters;

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                try
                {
                    var response = await request.PostAsync(uri, content);
                    string contentString = await response.Content.ReadAsStringAsync();
                    model.CognitiveAnalysis = Model.CognitiveAnalysis.FromJson(contentString);
                }
                catch (System.Exception e)
                {
                    this.logger.LogException(e);
                }
            }

            model.Brand = shopItem.Brand;
            model.Name = shopItem.Name;
            model.SerializedImage = shopItem.SerializedImage;
            model.Version = shopItem.Version;

            if (null != shopItem.Opinions && shopItem.Opinions.Any())
            {
                var modelOpinions = shopItem
                    .Opinions
                    .Select(o =>
                    {
                        var repository = this.entityStore.Create<Model.ShopItemOpinion>();
                        repository.Comment = o.Comment;
                        repository.Note = o.Note;
                        repository.ShopItemId = model.Id;
                        return repository;
                    });
                model.Opinions = model.Opinions ?? new List<Model.ShopItemOpinion>();
                model.Opinions.AddRange(modelOpinions);
            }

            this.entityStore.SaveChanges();
            return Ok(model);
        }
    }
}