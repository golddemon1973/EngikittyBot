using System.Collections.Concurrent;
using NetCord;
using NetCord.Rest;

namespace Engikitty.Bot
{
    public static class CooldownHandler
    {
        public static readonly int CooldownDurationSeconds = 3;

        private static readonly ConcurrentDictionary<ulong, long> Cooldowns = new();

        public static async Task<bool> DoCooldown(ApplicationCommandInteraction AppCmdInteraction, Interaction UserInteraction)
        {
            ulong UserId = UserInteraction.User.Id;
            long CurrentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (Cooldowns.TryGetValue(UserId, out long CooldownEndsAt) && CooldownEndsAt > CurrentTime)
            {
                await AppCmdInteraction.SendResponseAsync(InteractionCallback.Message(new InteractionMessageProperties()
                {
                    Flags = MessageFlags.Ephemeral,
                    Embeds = [
                        new EmbedProperties()
                        {
                            Thumbnail = new EmbedThumbnailProperties("https://cdn.discordapp.com/attachments/1505301024443994263/1525845208925868082/dizzy.jpg?ex=6a54dd96&is=6a538c16&hm=cd71842116a2e5187a9fea2720a659281b604b50dd3fbbd93aba3153fe64bc53&"),
                            Title = "Cooldown!!",
                            Description = $"You're going too fast! Please wait <t:{CooldownEndsAt}:R>.",
                            Color = new Color(255, 165, 0),
                            Timestamp = DateTimeOffset.UtcNow,
                        }
                    ]
                }));

                return true;
            }

            Cooldowns[UserId] = CurrentTime + CooldownDurationSeconds;
            return false;
        }
    }
}