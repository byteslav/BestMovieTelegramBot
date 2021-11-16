using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using BestMovie.Entities;
using BestMovie.Parser.Interfaces;

namespace BestMovie.Parser
{
    public class GenreParser : IParser<IEnumerable<Genre>>
    {
        public IEnumerable<Genre> Parse(IHtmlDocument document)
        {
            var items = document.QuerySelectorAll("a")
                .Where(i => i.ClassName == "gallery__nbl-tile nbl-tile nbl-tile_type_compact nbl-tile_style_aratus nbl-tile_hasAvatar_0 nbl-tile_hasIcon_1");
            var genres = items.Select(genre => new Genre { UrlPrefix = genre.GetAttribute("href"), Name = genre.TextContent }).ToList();
            genres.Remove(genres.Last());
            return genres;
        }
    }
}