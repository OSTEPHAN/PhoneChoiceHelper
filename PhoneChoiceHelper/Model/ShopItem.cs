
namespace PhoneChoiceHelper.Model
{
    using Dne.Core.Storage;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public partial class CognitiveAnalysis
    {
        [JsonProperty("categories")]
        public Category[] Categories { get; set; }

        [JsonProperty("tags")]
        public Tag[] Tags { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
    }

    public partial class Category
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("detail")]
        public Detail Detail { get; set; }
    }

    public partial class Detail
    {
        [JsonProperty("landmarks")]
        public object[] Landmarks { get; set; }
    }

    public partial class Description
    {
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("captions")]
        public Caption[] Captions { get; set; }
    }

    public partial class Caption
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }

    public partial class CognitiveAnalysis
    {
        public static CognitiveAnalysis FromJson(string json) => JsonConvert.DeserializeObject<CognitiveAnalysis>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CognitiveAnalysis self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }

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
        public CognitiveAnalysis CognitiveAnalysis { get; set; }
        public List<ShopItemOpinion> Opinions { get; set; }
        public List<ShopItemReview> Reviews { get; set; }
    }
}