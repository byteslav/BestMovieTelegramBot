using System.Threading.Tasks;
using AngleSharp.Html.Parser;

namespace BestMovie.Parser.Core
{
    public class ParserService<T> where T: class
    {
        private readonly IParser<T> _parser;
        private readonly IParserSettings _parserSettings;
        private readonly HtmlLoader _loader;

        public ParserService(IParser<T> parser, IParserSettings parserSettings)
        {
            _parser = parser;
            _parserSettings = parserSettings;
            _loader = new HtmlLoader(parserSettings);
        }

        public async Task<T> GetMoviesCollection()
        {
            var source = await _loader.GetSourceByGenre(_parserSettings.Genre);
            var htmlParser = new HtmlParser();

            var document = await htmlParser.ParseDocumentAsync(source);

            var result = _parser.Parse(document);

            return result;
        }
    }
}