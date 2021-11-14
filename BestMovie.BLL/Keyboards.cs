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
        
        public static ReplyKeyboardMarkup GetGenresKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Comedy"),                  
                new KeyboardButton("Drama")
            });
        }
    }
}