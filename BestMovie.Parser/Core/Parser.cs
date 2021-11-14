using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static BestMovie.Entities.Movie;

namespace BestMovie.Parser.Core
{
    public class Parser : IParser<IEnumerable<MovieListElement>>
    {
        public IEnumerable<MovieListElement> Parse(IHtmlDocument document)
        {
            var items = document.QuerySelectorAll("body")
                .Children("script")
                .Where(i => i.Attributes
                    .GetNamedItem("type") != null && i.Attributes
                    .GetNamedItem("type").Value.Equals("application/ld+json"));

            var movieCollection = ConvertJsonToMovieCollection(items.First().InnerHtml);

            return movieCollection;
        }

        private IEnumerable<MovieListElement> ConvertJsonToMovieCollection(string json)
        {
            var jObject = JObject.Parse(json);
            jObject.First.Remove();
            jObject.First.Remove();

            var deserializedObjects = JsonConvert.DeserializeObject<MovieRoot>(jObject.ToString());
            return deserializedObjects.MovieListElements;
        }
    }
}