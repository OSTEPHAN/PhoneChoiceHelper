
namespace PhoneChoiceHelper.Controllers
{
    using Dne.Core.Logging;
    using Dne.Core.Storage;
    using Dne.Web.Http;
    using Swashbuckle.Swagger.Annotations;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    [DisplayName("ShopItem matching")]
    [RoutePrefix("api/shopitem")]
    public class ShopItemController : ApiController
    {
        private readonly ILogger logger;
        private readonly IEntityStore entityStore;
        public ShopItemController(ILogger logger, IEntityStore entityStore)
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

        [HttpPost]
        [Route]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public IHttpActionResult Post(Model.ShopItem shopItem)
        {
            var model = this.entityStore.Create<Model.ShopItem>();
            model.Brand = shopItem.Brand;
            model.Name = shopItem.Name;
            model.SerializedImage = shopItem.SerializedImage;
            model.Version = shopItem.Version;

            model.Opinions = model.Opinions ?? new List<Model.ShopItemOpinion>();
            model.Reviews = model.Reviews ?? new List<Model.ShopItemReview>();

            var modelOpinions = shopItem
                .Opinions
                .Select(o =>
                {
                    var repository = this.entityStore.Create<Model.ShopItemOpinion>();
                    repository.Comment = o.Comment;
                    repository.Note = o.Note;
                    repository.ShopItemId = shopItem.Id;
                    return repository;
                });
            model.Opinions.AddRange(modelOpinions);

            var modeReviews = shopItem
                .Reviews
                .Select(r =>
                {
                    var repository = this.entityStore.Create<Model.ShopItemReview>();
                    repository.Url = r.Url;
                    repository.ShopItemId = shopItem.Id;
                    return repository;
                });
            model.Reviews.AddRange(modeReviews);
            model.Reviews.Add(new Model.ShopItemReview() { ShopItemId = shopItem.Id, Url = "http://www.lesnumeriques.com" });

            this.entityStore.SaveChanges();
            return Ok(model);
        }
    }
}