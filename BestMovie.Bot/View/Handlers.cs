using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestMovie.BLL;
using BestMovie.BLL.Services;
using BestMovie.Entities;
using BestMovie.Parser;
using BestMovie.Parser.Settings;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using static BestMovie.Entities.Movie;

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
                var text = "Please, wait...";
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken);
                GenresCollection.Genres = await GetGenres("movies");
                await messageBuilder.SendGenresByCategory(botClient, chatId, "movie", "Choose your movie genre: ", GenresCollection.Genres,
                    cancellationToken);
            }
            else if (textMessage == "what can you do?")
            {
                var text = "Bot info";
                var keyboard = Keyboards.GetMainKeyboard();
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, keyboard);
            }
            else if (GenresCollection.Genres.Exists(genre => genre.Name.ToLower().Equals(textMessage)))
            {
                var text = "Movies list is loading...";
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken);
                var genre = GenresCollection.Genres.Find(g => g.Name.ToLower().Equals(textMessage))?.UrlPrefix;
                var collection = await GetMoviesByGenre(genre);
                await messageBuilder.SendMoviesByGenre(botClient, chatId, textMessage,
                    collection, cancellationToken, Keyboards.GetMainKeyboard());
            }
            else
            {
                var text = "I can't resolve this phrase...";
                await messageBuilder.SendMessage(botClient, chatId, text, cancellationToken, Keyboards.GetMainKeyboard());
            }
        }
        private static async Task<IEnumerable<MovieListElement>> GetMoviesByGenre(string genre)
        {
            var parser = new MovieService<IEnumerable<MovieListElement>>(
                new MovieParser(),
                new MovieParserSettings(genre));

            var moviesCollection = await parser.GetMoviesCollection();
            return moviesCollection;
        }
        
        private static async Task<List<Genre>> GetGenres(string category)
        {
            var parser = new GenreService<IEnumerable<Genre>>(
                new GenreParser(),
                new GenreParserSettings(category));
            
            var genresCollection = await parser.GetGenresCollection();
            var genresList = genresCollection.ToList();
            return genresList;
        }
    }
}