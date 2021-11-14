namespace BestMovie.Parser.Core
{
    public class Settings : IParserSettings
    {
        public Settings(string genre)
        {
            Genre = genre;
        }
        public string BaseUrl { get; set; } =
            "https://www.ivi.ru/movies/";
        public string Prefix { get; set; } = "";
        public string Genre { get; set; }
    }
}