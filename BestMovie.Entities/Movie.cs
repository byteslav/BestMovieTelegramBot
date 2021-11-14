using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BestMovie.Entities
{
    [Serializable]
    public class Movie
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public string DateCreated { get; set; }
        
        public class MovieListElement
        {
            [JsonProperty("@type")]
            public string Type { get; set; }
            public string Position { get; set; }
            [JsonProperty("item")]
            public Movie Movie { get; set; }
        }

        public class MovieRoot
        {
            [JsonProperty("itemListElement")]
            public IEnumerable<MovieListElement> MovieListElements { get; set; }
        }
    }
}