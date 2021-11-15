using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace BestMovie.BLL
{
    public static class Keyboards
    {
        public static ReplyKeyboardMarkup GetMainKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("What can you do?"),                  
                new KeyboardButton("Start")
            });
        }
        
        public static ReplyKeyboardMarkup GetGenresKeyboard(IEnumerable<string> genreCollection)
        {
            var keyboard = genreCollection.Select(genre => new KeyboardButton(genre));
            return new ReplyKeyboardMarkup(keyboard);
        }
    }
}