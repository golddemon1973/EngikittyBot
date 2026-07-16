using Engikitty.Commands;
using NetCord;

namespace Engikitty.Bot
{
    public static class Library
    {
        public static string GetFullCommandName(SlashCommandInteraction AppCmdInteraction)
        {
            var Data = AppCmdInteraction.Data;
            string Name = Data.Name;

            if (Data.Options is { Count: > 0 } Options)
            {
                var FirstOption = Options[0];

                if (FirstOption.Type == ApplicationCommandOptionType.SubCommandGroup)
                {
                    Name += $" {FirstOption.Name}";

                    if (FirstOption.Options is { Count: > 0 } SubOptions &&
                        SubOptions[0].Type == ApplicationCommandOptionType.SubCommand)
                    {
                        Name += $" {SubOptions[0].Name}";
                    }
                }
                else if (FirstOption.Type == ApplicationCommandOptionType.SubCommand)
                {
                    Name += $" {FirstOption.Name}";
                }
            }

            return Name;
        }

        public static CommandInfo GetCommandInfo(ApplicationCommandInteraction AppCmdInteraction)
        {
            string CommandName = AppCmdInteraction switch
            {
                SlashCommandInteraction Slash => Library.GetFullCommandName(Slash),
                _ => AppCmdInteraction.Data.Name
            };

            CommandInfo CmdInfo = Info.Commands[CommandName];

            if (CmdInfo is null)
            {
                Logger.Error($"Couldn't find command info for command '{CommandName}' (a.k.a {AppCmdInteraction.Data.Name})!");

                throw new ArgumentNullException();
            }

            return CmdInfo;
        }
    }
}

