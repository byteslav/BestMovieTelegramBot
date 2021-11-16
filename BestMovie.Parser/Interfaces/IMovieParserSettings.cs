namespace BestMovie.Parser.Interfaces
{
    public interface IMovieParserSettings
    {
        string BaseUrl { get; set; }
        string Prefix { get; set; }
        string Genre { get; set; }
    }
}