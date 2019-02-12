using Newtonsoft.Json;
using System.Collections.Generic;

namespace MS.CatMatching.Entities
{
    public class CatsImageCollection
    {
        [JsonProperty("images")]
        public List<CatImage> CatImages { get; set; }
    }
}
