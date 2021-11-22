using System;
using System.Threading;
using System.Threading.Tasks;
using BestMovie.Bot.Controllers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace BestMovie.Bot.View
{
    public static class Handlers
    {
        private static readonly MessageController _messageController = new MessageController();
        
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
                await _messageController.SendGenresByCategory(botClient, chatId, cancellationToken);
            }
            else if (textMessage == Messages.BotInfoRequest.ToLower())
            {
                var text = Messages.BotInfoResponse;
                var keyboard = Keyboards.GetMainKeyboard();
                await _messageController.SendMessage(botClient, chatId, text, cancellationToken, keyboard);
            }
            else if (_messageController.IsGenreExist(textMessage))
            {
                var keyboard = Keyboards.GetMainKeyboard();
                await _messageController.SendMoviesByGenre(botClient, chatId, textMessage, cancellationToken, keyboard);
            }
            else
            {
                var text = Messages.CantResolve;
                await _messageController.SendMessage(botClient, chatId, text, cancellationToken, Keyboards.GetMainKeyboard());
            }
        }
    }
}