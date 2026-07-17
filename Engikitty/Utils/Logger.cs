/*

  Code is property of @youraveragekitty on Discord.

  Redistribution that does not follow the "BSD 3-Clause" License protecting the EngikittyBot project is not allowed.

*/

namespace Engikitty
{
    public static class Logger
    {
        public static void Log(string Message)
        {
            Console.WriteLine(Message);
        }

        public static void Warning(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine(Message);
            Console.ResetColor();
        }

        public static void Error(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(Message);
            Console.ResetColor();
        }
    }
}