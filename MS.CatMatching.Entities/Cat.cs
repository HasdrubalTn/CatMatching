using Newtonsoft.Json;
using System;
using System.Text;

namespace MS.CatMatching.Entities
{

    [JsonObject("cat")]
    public class Cat
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("votes")]
        public int Votes { get; set; }

        [JsonProperty("image")]
        public CatImage Image { get; set; }
    }
}
