namespace BestMovie.Parser.Interfaces
{
    public interface IGenreParserSettings
    {
        string BaseUrl { get; set; }
        string Prefix { get; set; }
        string Category { get; set; }
    }
}