/*

  Code is property of @youraveragekitty on Discord.

  Redistribution that does not follow the "BSD 3-Clause" License protecting the EngikittyBot project is not allowed.

*/

using Engikitty.Services;
using NetCord;

namespace Engikitty.Handlers
{
    /// <summary>
    /// Handler regarding users
    /// </summary>
    public static class UserHandler
    {
        /// <summary>
        /// Registers the user on the Neon database if they're not registered yet
        /// </summary>
        /// <param name="UserInteraction"></param>
        public static async Task Run(Interaction UserInteraction)
        {
            Dictionary<string, object>? UserDic =
                await Database.ReadAsync<Dictionary<string, object>>(UserInteraction.User.Id.ToString());

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