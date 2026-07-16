using Engikitty.Services;
using NetCord;

namespace Engikitty.Bot
{
    public static class UserHandler
    {
        public static async Task Run(Interaction UserInteraction)
        {
            Dictionary<string, object>? UserDic = await Database.ReadAsync<Dictionary<string, object>>(UserInteraction.User.Id.ToString());

            if (UserDic != null) return;

            UserDic = new()
            {
                ["IsOnCooldown"] = false,
                ["CooldownEndsAt"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await Database.WriteAsync(UserInteraction.User.Id.ToString(), UserDic);
        }
    }
}