using System.Collections.Generic;

namespace BestMovie.BLL
{
    public static class GenresCollection
    {
        public static List<string> Genres => new() { "comedy", "drama", "thriller", "adventures" };
    }
}