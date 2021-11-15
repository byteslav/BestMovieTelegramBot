using AngleSharp.Html.Dom;

namespace BestMovie.Parser.Core
{
    public interface IHtmlParser<T> where T: class
    {
        T Parse(IHtmlDocument document);
    }
}