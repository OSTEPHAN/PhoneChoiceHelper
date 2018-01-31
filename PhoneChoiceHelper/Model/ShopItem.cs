
namespace PhoneChoiceHelper.Model
{
    using Dne.Core.Storage;
    using System.Collections.Generic;

    public class ShopItemReview : IEntity
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ShopItemId { get; set; }
    }

    public class ShopItemOpinion : IEntity
    {
        public int Id { get; set; }
        public int Note { get; set; }
        public string Comment { get; set; }
        public int ShopItemId {get; set;}
    }

    public class ShopItem : IEntity
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Version{ get; set; }
        public string SerializedImage { get; set; }
        public List<ShopItemOpinion> Opinions { get; set; }
        public List<ShopItemReview> Reviews { get; set; }
    }
}