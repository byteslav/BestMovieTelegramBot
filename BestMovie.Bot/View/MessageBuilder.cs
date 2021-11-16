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
            string text, CancellationToken cancellationToken, ReplyKeyboardMarkup keyboard = null)
        {
            //keyboard ??= Keyboards.GetMainKeyboard();
            var result = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   text, 
                cancellationToken: cancellationToken,
                replyMarkup: keyboard);
        }
        
        public async Task SendGenresByCategory(ITelegramBotClient botClient, long chatId, string category,
            string text, IEnumerable<Genre> collection, CancellationToken cancellationToken)
        {
            await SendMessage(botClient, chatId, text, cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: ConvertCollectionGenreMessage(collection),
                cancellationToken: cancellationToken);
        }
        
        public async Task SendMoviesByGenre(ITelegramBotClient botClient, long chatId, string genre,
            IEnumerable<MovieListElement> collection, CancellationToken cancellationToken, ReplyKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   ConvertCollectionMovieMessage(collection, genre),
                cancellationToken: cancellationToken,
                replyMarkup: keyboard);
        }
        
        private string ConvertCollectionMovieMessage(IEnumerable<MovieListElement> collection, string genre)
        {
            var result = new StringBuilder($"Best movies by genre {genre}: \n");
            foreach (var item in collection)
            {
                result.Append($"{item.Position}) ");
                result.Append($"{item.Movie.Name} \n");
            }

            return result.ToString();
        }
        
        private string ConvertCollectionGenreMessage(IEnumerable<Genre> collection)
        {
            var result = new StringBuilder();
            foreach (var item in collection)
            {
                result.Append($"/{item.Name} \n");
            }

            return result.ToString();
        }
    }
}