using BestMovie.Parser.Interfaces;

namespace BestMovie.Parser.Settings
{
    public class MovieParserSettings : IMovieParserSettings
    {
        public MovieParserSettings(string genre)
        {
            Genre = genre;
        }
        public string BaseUrl { get; set; } =
            "https://www.ivi.ru";
        public string Prefix { get; set; } = "?ivi_rating_10_gte=9";
        public string Genre { get; set; }
    }
}