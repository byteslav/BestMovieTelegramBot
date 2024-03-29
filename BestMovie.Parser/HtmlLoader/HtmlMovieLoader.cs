﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BestMovie.Parser.Interfaces;

namespace BestMovie.Parser.HtmlLoader
{
    public class HtmlMovieLoader
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public HtmlMovieLoader(IMovieParserSettings settings)
        {
            _httpClient = new HttpClient();
            _url = $"{settings.BaseUrl}/{settings.Genre}{settings.Prefix}";
        }

        public async Task<string> GetSourceByGenre(string genre)
        {
            var currentUrl = _url.Replace("{CurrentGenre}", genre);
            var response = await _httpClient.GetAsync(currentUrl);
            string source = null;

            if (response is { StatusCode: HttpStatusCode.OK })
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}