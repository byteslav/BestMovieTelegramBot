using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BestMovie.Parser.Interfaces;

namespace BestMovie.Parser.HtmlLoader
{
    public class HtmlGenreLoader
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public HtmlGenreLoader(IGenreParserSettings settings)
        {
            _httpClient = new HttpClient();
            _url = $"{settings.BaseUrl}/{settings.Category}{settings.Prefix}";
        }

        public async Task<string> GetSourceByCategory(string category)
        {
            var currentUrl = _url.Replace("{CurrentCategory}", category);
            var response =  await _httpClient.GetAsync(currentUrl);
            string source = null;
            
            if (response is { StatusCode: HttpStatusCode.OK })
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}