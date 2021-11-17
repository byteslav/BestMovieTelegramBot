using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BestMovie.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static BestMovie.Entities.Movie;

namespace BestMovie.Bot.View
{
    public class MessageBuilder
    {
        private readonly int _maxImagesCount = 10;
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
            if (collection == null)
            {
                await SendMessage(botClient, chatId, Messages.GenreIsNotAvaliable, cancellationToken);
                return;
            }
            var messageAlbum = collection
                .Select(element => new InputMediaPhoto(new InputMedia(element.Movie.Image)))
                .Cast<IAlbumInputMedia>().Take(_maxImagesCount);
            
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   ConvertCollectionMovieMessage(collection, genre),
                cancellationToken: cancellationToken,
                replyMarkup: keyboard);
            await botClient.SendMediaGroupAsync(chatId, messageAlbum, cancellationToken: cancellationToken);
        }
        
        private string ConvertCollectionMovieMessage(IEnumerable<MovieListElement> collection, string genre)
        {
            var result = new StringBuilder($"{Messages.BestMovieByGenre}\n");
            foreach (var item in collection)
            {
                result.Append($"{item.Position}) ");
                result.Append($"{item.Movie.Name} \n");
            }

            return result.ToString();
        }
        
        private string ConvertCollectionGenreMessage(IEnumerable<Genre> collection)
        {
            var result = new StringBuilder("\n");
            foreach (var item in collection)
            {
                result.Append($"/{item.Name} \n");
            }

            return result.ToString();
        }
    }
}