namespace BestMovie.Parser.Core
{
    public interface IParserSettings
    {
        string BaseUrl { get; set; }
        string Prefix { get; set; }
        string Genre { get; set; }
    }
}