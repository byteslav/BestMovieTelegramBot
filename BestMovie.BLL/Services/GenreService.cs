﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using BestMovie.Entities;
using BestMovie.Parser.HtmlLoader;
using BestMovie.Parser.Interfaces;
using HtmlParser = AngleSharp.Html.Parser.HtmlParser;

namespace BestMovie.BLL.Services
{
    public class GenreService
    {
        private readonly IParser<IEnumerable<Genre>> _parser;
        private readonly IGenreParserSettings _genreParserSettings;
        private readonly HtmlGenreLoader _genreLoader;
        
        public IEnumerable<Genre> Genres { get; }

        public GenreService(IParser<IEnumerable<Genre>> parser, IGenreParserSettings genreParserSettings)
        {
            _parser = parser;
            _genreParserSettings = genreParserSettings;
            _genreLoader = new HtmlGenreLoader(genreParserSettings);

            Genres = GetGenresCollection().Result;
        }

        private async Task<IEnumerable<Genre>> GetGenresCollection()
        {
            var source = await _genreLoader.GetSourceByCategory(_genreParserSettings.Category);
            var htmlParser = new HtmlParser();

            var document = await htmlParser.ParseDocumentAsync(source);
            var result = _parser.Parse(document);

            return result;
        }
        
        public string GetGenreUrl(string genre)
        {
            var genreList = Genres.ToList();
            var genreUrl = genreList.Find(g => g.Name.ToLower().Equals(genre))?.UrlPrefix;

            return genreUrl;
        }
        
        public bool IsGenreExist(string genreName)
        {
            var genresList = Genres.ToList();
            return genresList.Exists(genre => genre.Name.ToLower().Equals(genreName));
        }
    }
}