using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestMovie.BLL;
using BestMovie.BLL.Services;
using BestMovie.Entities;
using BestMovie.Parser;
using BestMovie.Parser.Settings;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static BestMovie.Entities.Movie;

namespace BestMovie.Bot.Controllers
{
    public class MessageController
    {
        public async Task SendMessage(ITelegramBotClient botClient, long chatId,
            string text, CancellationToken cancellationToken, ReplyKeyboardMarkup keyboard = null)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   text, 
                cancellationToken: cancellationToken,
                replyMarkup: keyboard);
        }
        
        public async Task SendGenresByCategory(ITelegramBotClient botClient, long chatId, string category,
            string text, CancellationToken cancellationToken)
        {
            var genreService = new GenreService<IEnumerable<Genre>>(
                new GenreParser(),
                new GenreParserSettings(category));
            await SendMessage(botClient, chatId, text, cancellationToken);
            
            var collection = await genreService.GetGenresCollection();
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: Utility.ConvertCollectionGenreMessage(collection, Messages.PleaseWait),
                cancellationToken: cancellationToken);
        }
        
        public async Task SendMoviesByGenre(ITelegramBotClient botClient, long chatId, string genre, string category, string text,
             CancellationToken cancellationToken, ReplyKeyboardMarkup keyboard, int maxImagesCount = 10)
        {
            await SendMessage(botClient, chatId, text, cancellationToken);
            
            var genreUrl = await GetGenreUrl(genre, category);
            var movieService = new MovieService<IEnumerable<MovieListElement>>(
                new MovieParser(),
                new MovieParserSettings(genreUrl));
            
            var collection = await movieService.GetMoviesCollection();
            if (collection == null)
            {
                await SendMessage(botClient, chatId, Messages.GenreIsNotAvaliable, cancellationToken);
                return;
            }
            var messageAlbum = movieService.GetAlbumOfMovies(collection, maxImagesCount);
            
            await SendMessage(botClient, chatId, Utility.ConvertCollectionMovieMessage(collection, Messages.BestMovieByGenre), cancellationToken, keyboard);
            await botClient.SendMediaGroupAsync(chatId, messageAlbum, cancellationToken: cancellationToken);
        }
        
        public async Task<bool> IsGenreExist(string genreName, string category)
        {
            var genreService = new GenreService<IEnumerable<Genre>>(
                new GenreParser(),
                new GenreParserSettings(category));
            
            var genres = await genreService.GetGenresCollection();
            var genresList = genres.ToList();
            return genresList.Exists(genre => genre.Name.ToLower().Equals(genreName));
        }

        private async Task<string> GetGenreUrl(string genre, string category)
        {
            var genreService = new GenreService<IEnumerable<Genre>>(
                new GenreParser(),
                new GenreParserSettings(category));

            var genres = await genreService.GetGenresCollection();
            var genreList = genres.ToList();
            var genreUrl = genreList.Find(g => g.Name.ToLower().Equals(genre))?.UrlPrefix;

            return genreUrl;
        }
    }
}