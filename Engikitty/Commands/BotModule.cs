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
    /// Command module for every "bot" command
    /// </summary>
    [SlashCommand("bot", "All sorts of tools regarding engikitty itself",
        Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel])]
    public class BotModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        /// <summary>
        /// Returns the bot's interaction ping
        /// </summary>
        [SubSlashCommand("ping", "Returns the bot's ping")]
        public async Task Ping()
        {
            await Context.Interaction.ModifyResponseAsync(Message =>
            {
                Message.Embeds =
                [
                    new EmbedProperties()
                    {
                        Thumbnail = new EmbedThumbnailProperties(
                            "https://cdn.discordapp.com/attachments/1505301024443994263/1525883632714121226/throwbrick.gif?ex=6a55015f&is=6a53afdf&hm=dbf99c0e10bb0f93932e8fce83180c6c2f507637477056c9555e46d00fec52eb&"),
                        Title = "Pong!!",
                        Description = $"Latency is {Context.Client.Latency.TotalMilliseconds}ms",
                        Color = new Color(46, 111, 64),
                        Timestamp = DateTimeOffset.UtcNow,
                    }
                ];
            });
        }
    }
}