using System.Threading;
using System.Threading.Tasks;
using BestMovie.BLL;
using BestMovie.BLL.Services;
using BestMovie.Parser;
using BestMovie.Parser.Settings;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BestMovie.Bot.Controllers
{
    public class MessageController
    {
        private readonly GenreService _genreService;
        public MessageController()
        {
            _genreService = new GenreService(
                new GenreParser(),
                new GenreParserSettings("movies"));
        }
        
        public async Task SendMessage(ITelegramBotClient botClient, long chatId,
            string text, CancellationToken cancellationToken, ReplyKeyboardMarkup keyboard = null)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   text, 
                cancellationToken: cancellationToken,
                replyMarkup: keyboard);
        }
        
        public async Task SendGenresByCategory(ITelegramBotClient botClient, long chatId,
            CancellationToken cancellationToken)
        {
            await SendMessage(botClient, chatId, Messages.PleaseWait, cancellationToken);
            
            var collection = _genreService.Genres;
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: Utility.ConvertCollectionGenreMessage(collection, Messages.Genres),
                cancellationToken: cancellationToken);
        }
        
        public async Task SendMoviesByGenre(ITelegramBotClient botClient, long chatId, string genre,
             CancellationToken cancellationToken, ReplyKeyboardMarkup keyboard, int maxImagesCount = 10)
        {
            await SendMessage(botClient, chatId, Messages.MoviesIsLoading, cancellationToken);
            
            var genreUrl = _genreService.GetGenreUrl(genre);
            var movieService = new MovieService(
                new MovieParser(),
                new MovieParserSettings(genreUrl));
            
            var collection = await movieService.GetMoviesCollection();
            if (collection.Count == 0)
            {
                await SendMessage(botClient, chatId, Messages.GenreIsNotAvaliable, cancellationToken);
                return;
            }
            var messageAlbum = movieService.GetAlbumOfMovies(collection, maxImagesCount);
            
            await SendMessage(botClient, chatId, Utility.ConvertCollectionMovieMessage(collection, Messages.BestMovieByGenre), cancellationToken, keyboard);
            await botClient.SendMediaGroupAsync(chatId, messageAlbum, cancellationToken: cancellationToken);
        }
        
        public bool IsGenreExist(string genreName)
        {
            return _genreService.IsGenreExist(genreName);
        }
    }
}