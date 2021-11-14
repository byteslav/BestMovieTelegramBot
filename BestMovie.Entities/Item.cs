// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

using Newtonsoft.Json;

namespace BestMovie.Entities
{
    public class Item
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        public string dateCreated { get; set; }
    }
}