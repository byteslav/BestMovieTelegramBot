using System;
using System.Threading;
using System.Threading.Tasks;
using BestMovie.BLL;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace BestMovie.Bot.View
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
            var textMessage = message.Text.ToLower();
            
            Console.WriteLine($"Received a '{textMessage}' message in chat {chatId}.");
            
            var messageBuilder = new MessageBuilder();
            if (textMessage == "start")
            {
                var text = "Choose movie genre";
                var keyboard = Keyboards.GetGenresKeyboard(GenresCollection.Genres);
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, keyboard);
            }
            else if (textMessage == "what can you do?")
            {
                var text = "Bot info";
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, Keyboards.GetMainKeyboard());
            }
            else if (GenresCollection.Genres.Contains(textMessage))
            {
                var text = "Please wait...";
                await messageBuilder.SendMoviesByCategory(botClient, chatId, textMessage, text,
                    cancellationToken, Keyboards.GetMainKeyboard());
            }
            else
            {
                var text = "I can't resolve this phrase...";
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, Keyboards.GetMainKeyboard());
            }
        }
    }
}