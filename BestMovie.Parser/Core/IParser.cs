using AngleSharp.Html.Dom;

namespace BestMovie.Parser.Core
{
    public interface IParser<T> where T: class
    {
        T Parse(IHtmlDocument document);
    }
}