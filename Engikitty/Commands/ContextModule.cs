/*

  Code is property of @youraveragekitty on Discord.

  Redistribution that does not follow the "BSD 3-Clause" License protecting the EngikittyBot project is not allowed.

*/

using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Engikitty.Commands
{
    /// <summary>
    /// Command module for every contextual command, such as message commands or user commands
    /// </summary>
    public class ContextModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        /// <summary>
        /// Bad-translates text 5 times
        /// </summary>
        /// <param name="Msg">Message used to extract content</param>
        [MessageCommand("Bad Translate (5 times)",
            Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel])]
        public async Task BadTranslate5(RestMessage Msg)
        {
            await Bot.Library.Commands.DoMessageBadTranslate(Msg.Content, 5, Context);
        }

        /// <summary>
        /// Bad-translates text 10 times
        /// </summary>
        /// <param name="Msg">Message used to extract content</param>
        [MessageCommand("Bad Translate (10 times)",
            Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel])]
        public async Task BadTranslate10(RestMessage Msg)
        {
            await Bot.Library.Commands.DoMessageBadTranslate(Msg.Content, 10, Context);
        }

        /// <summary>
        /// Bad-translates text 20 times
        /// </summary>
        /// <param name="Msg">Message used to extract content</param>
        [MessageCommand("Bad Translate (20 times)",
            Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel])]
        public async Task BadTranslate20(RestMessage Msg)
        {
            await Bot.Library.Commands.DoMessageBadTranslate(Msg.Content, 20, Context);
        }
    }
}