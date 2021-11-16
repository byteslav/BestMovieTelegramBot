using AngleSharp.Html.Dom;

namespace BestMovie.Parser.Interfaces
{
    public interface IParser<T> where T: class
    {
        T Parse(IHtmlDocument document);
    }
}