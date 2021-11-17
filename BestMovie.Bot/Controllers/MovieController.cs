using System.Collections.Generic;
using System.Threading.Tasks;
using BestMovie.BLL.Services;
using BestMovie.Parser;
using BestMovie.Parser.Settings;
using static BestMovie.Entities.Movie;

namespace BestMovie.Bot.Controllers
{
    public class MovieController
    {
        public async Task<IEnumerable<MovieListElement>> GetMoviesByGenre(string genre)
        {
            var parser = new MovieService<IEnumerable<MovieListElement>>(
                new MovieParser(),
                new MovieParserSettings(genre));

            var moviesCollection = await parser.GetMoviesCollection();
            return moviesCollection;
        }
    }
}