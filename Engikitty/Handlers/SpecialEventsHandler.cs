/*

  Code is property of @youraveragekitty on Discord.

  Redistribution that does not follow the "BSD 3-Clause" License protecting the EngikittyBot project is not allowed.

*/

using Engikitty.Services;
using System.Text.Json;
using NetCord;
using NetCord.Rest;

namespace Engikitty.Handlers
{
    /// <summary>
    /// A special event
    /// </summary>
    public class SpecialEvent
    {
        public readonly Dictionary<string, object> Settings;

        public SpecialEvent(Dictionary<string, Object> _Settings)
        {
            Settings = _Settings;
        }
    }

    /// <summary>
    /// Handler for special events
    /// </summary>
    public static class SpecialEventsHandler
    {
        #region Events

        /// <summary>
        /// Currently registered special events
        /// </summary>
        public static readonly Dictionary<string, SpecialEvent> SpecialEvents = new()
        {
            ["NewUser"] = new SpecialEvent(new Dictionary<string, object>()
            {
                ["Once"] = true,
                ["Action"] = async (ApplicationCommandInteraction AppCmdInteraction, Interaction _) =>
                {
                    await AppCmdInteraction.SendFollowupMessageAsync(new InteractionMessageProperties()
                    {
                        Flags = MessageFlags.Ephemeral,
                        Embeds =
                        [
                            new EmbedProperties()
                            {
                                Thumbnail = new EmbedThumbnailProperties(
                                    "https://cdn.discordapp.com/attachments/1505301024443994263/1525878485896265840/release.jpg?ex=6a54fc94&is=6a53ab14&hm=4a586739a6960870b68bf1fd6c6a386dfaab8fd0828246adffd7897be702403e&"),
                                Title = "Hiiii!!!",
                                Description = "Thanks for using Engikitty c:",
                                Color = new Color(0, 255, 0),
                                Timestamp = DateTimeOffset.UtcNow,
                            }
                        ]
                    });
                }
            })
        };

        #endregion

        /// <summary>
        /// Helper
        /// </summary>
        /// <param name="Value">Object to check</param>
        /// <returns>A bool</returns>
        private static bool IsTrue(object? Value) =>
            Value switch
            {
                bool B => B,
                JsonElement { ValueKind: JsonValueKind.True } => true,
                _ => false
            };

        /// <summary>
        /// Check if the user has an awaiting special event to check
        /// </summary>
        /// <param name="AppCmdInteraction"></param>
        /// <param name="UserInteraction"></param>
        public static async Task Run(ApplicationCommandInteraction AppCmdInteraction, Interaction UserInteraction)
        {
            Dictionary<string, object>? UserDic =
                await Database.ReadAsync<Dictionary<string, object>>(UserInteraction.User.Id.ToString());

            if (UserDic == null)
            {
                Logger.Warning("NO USERDIC WTF???");

                return;
            }

            foreach (var (EventName, Event) in SpecialEvents)
            {
                bool IsOnce = Event.Settings.TryGetValue("Once", out var OnceValue) && IsTrue(OnceValue);
                string DoneKey = $"Done_{EventName}";

                if (IsOnce && UserDic.TryGetValue(DoneKey, out var DoneValue) && IsTrue(DoneValue))
                {
                    continue;
                }

                if (Event.Settings.TryGetValue("Action", out var ActionObj) &&
                    ActionObj is Func<ApplicationCommandInteraction, Interaction, Task> Action)
                {
                    try
                    {
                        await Action(AppCmdInteraction, UserInteraction);
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error($"SpecialEvent '{EventName}' action failed:\n\n{Ex}");
                    }
                }

                if (IsOnce)
                {
                    UserDic[DoneKey] = true;
                    await Database.WriteAsync(UserInteraction.User.Id.ToString(), UserDic);
                }
            }
        }
    }
}