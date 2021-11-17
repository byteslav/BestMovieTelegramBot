using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestMovie.Bot.Controllers;
using BestMovie.Entities;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace BestMovie.Bot.View
{
    public static class Handlers
    {
        private static List<Genre> _genres;
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            if (message == null) return;
            
            var chatId = message.Chat.Id;
            var textMessage = message.Text.ToLower();


            Console.WriteLine($"Received a '{textMessage}' message in chat {chatId}.");
            
            var messageBuilder = new MessageBuilder();
            var genreController = new GenreController();
            var movieController = new MovieController();

            if (textMessage == "start")
            {
                var text = "Please, wait...";
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken);
                _genres = await genreController.GetGenres("movies");
                await messageBuilder.SendGenresByCategory(botClient, chatId, "movie", "Choose your movie genre: ", _genres,
                    cancellationToken);
            }
            else if (textMessage == "what can you do?")
            {
                var text = "Bot info";
                var keyboard = Keyboards.GetMainKeyboard();
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, keyboard);
            }
            else if (_genres != null && _genres.Exists(genre => genre.Name.ToLower().Equals(textMessage)))
            {
                var text = "Movies list is loading...";
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken);
                var genre = _genres.Find(g => g.Name.ToLower().Equals(textMessage))?.UrlPrefix;
                var collection = await movieController.GetMoviesByGenre(genre);
                await messageBuilder.SendMoviesByGenre(botClient, chatId, textMessage,
                    collection, cancellationToken, Keyboards.GetMainKeyboard());
            }
            else
            {
                var text = "I can't resolve this phrase...";
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, Keyboards.GetMainKeyboard());
            }
        }
    }
}