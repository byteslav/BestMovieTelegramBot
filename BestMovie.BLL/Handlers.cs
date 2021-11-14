using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestMovie.Parser.Core;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using static BestMovie.Entities.Movie;

namespace BestMovie.BLL
{
    public static class Handlers
    {
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
    
            Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}.");

            switch (message.Text.ToLower())
            {
                case "start":
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text:   "Choose movie genre", 
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboards.GetGenresKeyboard());
                    break;
                case "what can you do?":
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text:   "Bot info",
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboards.GetMainKeyboard());
                    break;
                default:
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text:   "I can't resolve this phrase", 
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboards.GetMainKeyboard());
                    break;
            }
        }

        public static async void GetMoviesByGenre(string genre)
        {
            var parser = new ParserService<IEnumerable<MovieListElement>>(
                new Parser.Core.Parser(),
                new Settings(genre));

            var test = await parser.GetMoviesCollection();
        }
    }
}