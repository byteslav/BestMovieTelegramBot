using System.Linq;
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
        
        public async Task SendGenresByCategory(ITelegramBotClient botClient, long chatId, string category,
            string text, CancellationToken cancellationToken)
        {
            
            await SendMessage(botClient, chatId, text, cancellationToken);
            
            var collection = _genreService.Genres;
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
        
        public async Task<bool> IsGenreExist(string genreName, string category)
        {
            var genresList = _genreService.Genres.ToList();
            return genresList.Exists(genre => genre.Name.ToLower().Equals(genreName));
        }

        private async Task<string> GetGenreUrl(string genre, string category)
        {
            var genreList = _genreService.Genres.ToList();
            var genreUrl = genreList.Find(g => g.Name.ToLower().Equals(genre))?.UrlPrefix;

            return genreUrl;
        }
    }
}