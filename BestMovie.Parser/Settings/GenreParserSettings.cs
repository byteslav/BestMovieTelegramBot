using BestMovie.Parser.Interfaces;

namespace BestMovie.Parser.Settings
{
    public class GenreParserSettings : IGenreParserSettings
    {
        public GenreParserSettings(string category)
        {
            Category = category;
        }
        public string BaseUrl { get; set; } = "https://www.ivi.ru";
        public string Prefix { get; set; }
        public string Category { get; set; }
    }
}