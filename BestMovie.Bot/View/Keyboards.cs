using System.Collections.Generic;
using System.Linq;
using BestMovie.Entities;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BestMovie.Bot.View
{
    public static class Keyboards
    {
        public static ReplyKeyboardMarkup GetMainKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton(Messages.BotInfoRequest),                  
                new KeyboardButton(Messages.Start)
            });
        }
        
        public static ReplyKeyboardMarkup GetGenresKeyboard(IEnumerable<Genre> genreCollection)
        {
            var keyboard = genreCollection.Select(genre => new KeyboardButton(genre.Name));
            return new ReplyKeyboardMarkup(keyboard);
        }
    }
}