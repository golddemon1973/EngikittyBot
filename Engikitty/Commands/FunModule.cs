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
    /// Command module for every "fun" command
    /// </summary>
    [SlashCommand("fun", "A lot of stuff for fun, probably?",
        Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel])]
    public class FunModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        /// <summary>
        /// Bad-translates text a certain amount of times
        /// </summary>
        /// <param name="Text">The text to bad-translate</param>
        /// <param name="Times">The amount of times to do it</param>
        [SubSlashCommand("badtranslate", "Translate something. But like a lot.")]
        public async Task BadTranslate(
            [SlashCommandParameter(Name = "text", Description = "The text to translate", MaxLength = 768)]
            string Text,
            [SlashCommandParameter(Name = "loops", Description = "Times to loop", MinValue = 1, MaxValue = 100)]
            int Times = 5)
        {
            await Bot.Library.Commands.DoBadTranslate(Text, Times, Context);
        }

        /// <summary>
        /// Ask the digital 8ball a question
        /// </summary>
        /// <param name="Question">Question to ask</param>
        [SubSlashCommand("8ball", "Ask the 8 ball a question.")]
        public async Task Ask8Ball(
            [SlashCommandParameter(Name = "question", Description = "The question to ask the 8Ball", MaxLength = 1024)]
            string Question)
        {
            string Answer =
                Bot.Library.Commands.EightBallResponses[
                    new Random().Next(Bot.Library.Commands.EightBallResponses.Length)];

            await Context.Interaction.ModifyResponseAsync(Message =>
            {
                Message.Embeds =
                [
                    new EmbedProperties()
                    {
                        Thumbnail = new EmbedThumbnailProperties(
                            "https://cdn.discordapp.com/attachments/1505301024443994263/1526178240568229958/bleh.jpg?ex=6a5613bf&is=6a54c23f&hm=ea363ec0295c9090ccdefbafa73d3a015b4a54ece56661665750e21e4bd5ea3b&"),
                        Title = "Done!!",
                        Description = "THE FUCKASS 8BALL HAS SPOKEN",
                        Fields = new List<EmbedFieldProperties>()
                        {
                            new()
                            {
                                Name = "Question",
                                Value = Question,
                                Inline = false,
                            },

                            new()
                            {
                                Name = "Answer",
                                Value = Answer,
                            }
                        },
                        Color = new Color(46, 111, 64),
                        Timestamp = DateTimeOffset.UtcNow,
                    }
                ];
            });
        }
    }
}