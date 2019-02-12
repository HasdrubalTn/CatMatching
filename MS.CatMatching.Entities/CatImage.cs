using Newtonsoft.Json;

namespace MS.CatMatching.Entities
{
    public class CatImage
    {
        [JsonProperty("id")]
        public string ExternalId { get; set; }

        [JsonProperty("url")]
        public string ImageUrl { get; set; }
    }
}
