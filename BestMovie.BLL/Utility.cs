using System.Collections.Generic;
using System.Text;
using BestMovie.Entities;

namespace BestMovie.BLL
{
    public static class Utility
    {
        public static string ConvertCollectionMovieMessage(IEnumerable<Movie.MovieListElement> collection, string message)
        {
            var result = new StringBuilder($"{message}\n");
            foreach (var item in collection)
            {
                result.Append($"{item.Position}) ");
                result.Append($"{item.Movie.Name} ({item.Movie.DateCreated})\n");
            }

            return result.ToString();
        }
        
        public static string ConvertCollectionGenreMessage(IEnumerable<Genre> collection, string message)
        {
            var result = new StringBuilder($"{message}\n");
            foreach (var item in collection)
            {
                result.Append($"/{item.Name} \n");
            }

            return result.ToString();
        }
    }
}