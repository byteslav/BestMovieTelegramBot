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
        private static readonly MessageBuilder _messageBuilder = new MessageBuilder();
        private static readonly GenreController _genreController = new GenreController();
        private static readonly MovieController _movieController = new MovieController();
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


            Console.WriteLine(Messages.ConsoleHandleMessage, textMessage, chatId);

            if (textMessage == Messages.Start.ToLower())
            {
                var text = Messages.PleaseWait;
                await _messageBuilder.SendMessage(botClient, chatId, text, cancellationToken);
                _genres = await _genreController.GetGenres("movies");
                await _messageBuilder.SendGenresByCategory(botClient, chatId, "movie", Messages.ChooseGenre, _genres,
                    cancellationToken);
            }
            else if (textMessage == Messages.BotInfoRequest.ToLower())
            {
                var text = Messages.BotInfoResponse;
                var keyboard = Keyboards.GetMainKeyboard();
                await _messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, keyboard);
            }
            else if (_genres != null && _genres.Exists(genre => genre.Name.ToLower().Equals(textMessage)))
            {
                var text = Messages.MoviesIsLoading;
                await _messageBuilder.SendMessage(botClient, chatId, text, cancellationToken);
                var genre = _genres.Find(g => g.Name.ToLower().Equals(textMessage))?.UrlPrefix;
                var collection = await _movieController.GetMoviesByGenre(genre);
                await _messageBuilder.SendMoviesByGenre(botClient, chatId, textMessage,
                    collection, cancellationToken, Keyboards.GetMainKeyboard());
            }
            else
            {
                var text = Messages.CantResolve;
                await _messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, Keyboards.GetMainKeyboard());
            }
        }
    }
}