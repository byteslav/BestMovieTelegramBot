using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BestMovie.BLL.Services;
using BestMovie.Entities;
using BestMovie.Parser;
using BestMovie.Parser.Settings;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static BestMovie.Entities.Movie;

namespace BestMovie.Bot.View
{
    public class MessageBuilder
    {
        public async Task SendMessage(ITelegramBotClient botClient, long chatId,
            string text, CancellationToken cancellationToken, ReplyKeyboardMarkup keyboard)
        {
            var result = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   text, 
                cancellationToken: cancellationToken,
                replyMarkup: keyboard);
        }
        
        public async Task SendMoviesByCategory(ITelegramBotClient botClient, long chatId, string genre,
            string text, CancellationToken cancellationToken, ReplyKeyboardMarkup keyboard)
        {
            await SendMessage(botClient, chatId, text, cancellationToken, keyboard);
            
            var collection = await GetMoviesByGenre(genre);
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   ConvertCollectionToString(collection, genre),
                cancellationToken: cancellationToken,
                replyMarkup: keyboard);
        }
        
        private string ConvertCollectionToString(IEnumerable<MovieListElement> collection, string genre)
        {
            var result = new StringBuilder($"Best movies by genre {genre}: \n");
            foreach (var item in collection)
            {
                result.Append($"{item.Position}) ");
                result.Append($"{item.Movie.Name} \n");
            }

            return result.ToString();
        }
        
        private async Task<IEnumerable<MovieListElement>> GetMoviesByGenre(string genre)
        {
            var parser = new MovieService<IEnumerable<MovieListElement>>(
                new MovieParser(),
                new MovieParserSettings(genre));

            var moviesCollection = await parser.GetMoviesCollection();
            return moviesCollection;
        }
        
        private async Task<IEnumerable<Genre>> GetGenre(string category)
        {
            var parser = new GenreService<IEnumerable<Genre>>(
                new GenreParser(),
                new GenreParserSettings(category));
            
            var genresCollection = await parser.GetGenresCollection();
            return genresCollection;
        }
    }
}