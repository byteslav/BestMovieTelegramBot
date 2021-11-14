using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading;
using BestMovie.BLL;
using BestMovie.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BestMovieTelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Handlers.GetMoviesByGenre("comedy");
            var botClient = new TelegramBotClient(Configuration.BotToken);
            
            using var cts = new CancellationTokenSource();
            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            botClient.StartReceiving(
                new DefaultUpdateHandler(Handlers.HandleUpdateAsync, Handlers.HandleErrorAsync),
                cts.Token);
            
            Console.ReadLine();
            // Send cancellation request to stop bot
            cts.Cancel();
            
            var path = "C:/MyProjects/AspNet/BestMovie/BestMovie.Parser/test.json";
            var jsonString = File.ReadAllText(path);
            
        }
    }
}
