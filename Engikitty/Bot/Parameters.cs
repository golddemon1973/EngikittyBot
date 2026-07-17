using Engikitty.Types;

namespace Engikitty.Bot
{
    /// <summary>
    /// Class containing general info regarding the bot
    /// (Commands, version, etc)
    /// </summary>
    public static class Info
    {
        /// <summary>
        /// Dictionary containing CommandInfo class instances,
        /// used notably in Bot.cs to know if a command is ephemeral and/or is heavy
        /// </summary>
        public static readonly Dictionary<string, CommandInfo> Commands = new()
        {
            // Bot
            ["bot ping"] = new(),

            // Fun

            ["fun badtranslate"] = new(false, true),
            ["fun 8ball"] = new(),

            // Contextual

            ["Bad Translate (5 times)"] = new(false, true),
            ["Bad Translate (10 times)"] = new(false, true),
            ["Bad Translate (20 times)"] = new(false, true),
        };
    }
}