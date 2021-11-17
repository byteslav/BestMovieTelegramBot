using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovie.BLL.Services;
using BestMovie.Entities;
using BestMovie.Parser;
using BestMovie.Parser.Settings;

namespace BestMovie.Bot.Controllers
{
    public class GenreController
    {
        public async Task<List<Genre>> GetGenres(string category)
        {
            var parser = new GenreService<IEnumerable<Genre>>(
                new GenreParser(),
                new GenreParserSettings(category));
            
            var genresCollection = await parser.GetGenresCollection();
            var genresList = genresCollection.ToList();
            return genresList;
        }
    }
}