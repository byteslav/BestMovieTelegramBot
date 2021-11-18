using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using BestMovie.Entities;
using BestMovie.Parser.HtmlLoader;
using BestMovie.Parser.Interfaces;
using HtmlParser = AngleSharp.Html.Parser.HtmlParser;

namespace BestMovie.BLL.Services
{
    public class GenreService<T> where T: class
    {
        private readonly IParser<T> _parser;
        private readonly IGenreParserSettings _genreParserSettings;
        private readonly HtmlGenreLoader _genreLoader;
        
        public Task<T> Genres => GetGenresCollection();

        public GenreService(IParser<T> parser, IGenreParserSettings genreParserSettings)
        {
            _parser = parser;
            _genreParserSettings = genreParserSettings;
            _genreLoader = new HtmlGenreLoader(genreParserSettings);
        }

        public async Task<T> GetGenresCollection()
        {
            var source = await _genreLoader.GetSourceByCategory(_genreParserSettings.Category);
            var htmlParser = new HtmlParser();

            var document = await htmlParser.ParseDocumentAsync(source);
            var result = _parser.Parse(document);

            return result;
        }
    }
}