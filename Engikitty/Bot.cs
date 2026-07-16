/*
 
  Code is property of @youraveragekitty on Discord.
  
  Redistribution is not allowed.
 
*/

using Engikitty.Bot;
using Engikitty.Commands;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using EventHandler = Engikitty.Bot.EventHandler;

namespace Engikitty
{
  public static class Program
  {
    public static readonly bool DEBUG = true;
    
    public static async Task Main()
    {
      string? AUTH = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN_ENGIKITTY");

      if (AUTH == null) throw new ArgumentNullException(nameof(AUTH), "No bot token provided");

      GatewayClient BotClient;
      ApplicationCommandService<ApplicationCommandContext> CommandService;

      Logger.Log("Loading NetCord...");

      try
      {
        BotToken BotAuthToken = new(AUTH);
        GatewayClientConfiguration BotClientConfig = new()
        {
          Intents = GatewayIntents.Guilds | GatewayIntents.DirectMessages | GatewayIntents.MessageContent
        };
        
        BotClient = new(BotAuthToken, BotClientConfig);
        CommandService = new();
      }
      catch (Exception WentWrong)
      {
        Logger.Error("Could not load NetCord:\n\n" + WentWrong);

        return;
      }

      Logger.Log("Loaded NetCord!");
      Logger.Log("Loading Engikitty...");

      try
      {
        CommandService.AddModules(typeof(Program).Assembly);
        
        BotClient.Ready += async _ =>
        {
          await BotClient.UpdatePresenceAsync(new PresenceProperties(UserStatusType.Online)
          {
            Activities = [
              new UserActivityProperties("gay", UserActivityType.Custom)
              {
                State = "i am so i am",
              }
            ]
          });
          
          IReadOnlyList<ApplicationCommand> Registered = await CommandService.RegisterCommandsAsync(BotClient.Rest, BotClient.Id);
          
          foreach (ApplicationCommand Cmd in Registered)
          {
            Logger.Log($" - {Cmd.Name} (Type: {Cmd.Type})");
          }
        };

        BotClient.InteractionCreate += async UserInteraction =>
        {
          _ = Task.Run(async () =>
          {
            try
            {
              if (UserInteraction is not ApplicationCommandInteraction AppCmdInteraction) return;

              bool IsOnCooldown = await CooldownHandler.DoCooldown(AppCmdInteraction, UserInteraction);
              if (IsOnCooldown) return;

              CommandInfo CmdInfo = Library.GetCommandInfo(AppCmdInteraction);

              await UserInteraction.SendResponseAsync(InteractionCallback.DeferredMessage(
                CmdInfo.IsEphemeral ? MessageFlags.Ephemeral : null));

              if (CmdInfo.IsHeavy)
              {
                await AppCmdInteraction.ModifyResponseAsync(Message =>
                {
                  Message.Embeds =
                  [
                    new EmbedProperties()
                    {
                      Thumbnail = new EmbedThumbnailProperties(
                        "https://cdn.discordapp.com/attachments/1505301024443994263/1526184246153052311/engikittyHAMburher.gif?ex=6a561957&is=6a54c7d7&hm=9a1576387d50467f38ed0065c197e5da52d0ddd30dd34aac9ff09eeed99495d2&"),
                      Title = "Working on it..",
                      Description = "Engikitty is working. He's working. Like really hard.",
                      Color = new Color(130, 200, 229),
                      Timestamp = DateTimeOffset.UtcNow,
                    }
                  ];
                });
              }

              await UserHandler.Run(UserInteraction);

              IExecutionResult Result =
                await CommandService.ExecuteAsync(new ApplicationCommandContext(AppCmdInteraction, BotClient));

              if (Result is IFailResult FailResult)
              {
                Logger.Error("Our engineer sucks. Couldn't fix that:\n\n" + FailResult.Message);

                await AppCmdInteraction.ModifyResponseAsync(Message => Message.WithEmbeds([
                  new EmbedProperties()
                  {
                    Thumbnail = new EmbedThumbnailProperties(
                      "https://cdn.discordapp.com/attachments/1505301024443994263/1526183398345937006/DEATH.gif?ex=6a56188d&is=6a54c70d&hm=cf37986a75ea11b0a09f200d60f94450e005d7e24568d87385d0ba8abe5023c5&"),
                    Title = "Failed :c",
                    Description = "Couldn't execute this command.. Send this to the dev!\n\n```" + FailResult.Message +
                                  "```",
                    Color = new Color(255, 0, 0),
                    Timestamp = DateTimeOffset.UtcNow,
                  }
                ]));

                return;
              }

              await EventHandler.Run(AppCmdInteraction, UserInteraction);
            }
            catch (Exception WentWrong)
            {
              Logger.Error(WentWrong.ToString());
            }
          });

          await Task.CompletedTask;
        };
      }
      catch (Exception WentWrong)
      {
        Logger.Error("Could not load Engikitty:\n\n" + WentWrong);
      }

      Logger.Log("Loaded Engikitty!");

      // Done!!

      if (Environment.GetEnvironmentVariable("PORT") != null)
      {
        StartDummyServer();
      }
      
      await BotClient.StartAsync();

      Logger.Log("Everything loaded successfully :3");
      
      await Task.Delay(-1);
    }
    
    private static void StartDummyServer()
    {
      Task.Run(() =>
      {
        try
        {
          string PortStr = Environment.GetEnvironmentVariable("PORT") ?? "8080";
	  int.TryParse(PortStr, out int Port)

          TcpListener Listener = new(IPAddress.Any, Port);
          Listener.Start();

          Logger.Log($"[Web Server] Dummy server listening on port {Port}");

          while (true)
          {
            try
            {
              using TcpClient Client = Listener.AcceptTcpClient();
              using NetworkStream Stream = Client.GetStream();

              byte[] Buffer = new byte[1024];
              
              _ = Stream.Read(Buffer, 0, Buffer.Length);

              string Response = "HTTP/1.1 200 OK\r\n" +
                                "Content-Type: text/plain\r\n" +
                                "Connection: close\r\n" +
                                "Content-Length: 12\r\n\r\n" +
                                "Bot is alive!";
                                
              byte[] ResponseBytes = Encoding.UTF8.GetBytes(Response);
              Stream.Write(ResponseBytes, 0, ResponseBytes.Length);
            }
            catch (Exception Ex)
            {
              if (Debug) Logger.Error($"[Web Server Error]: {Ex.Message}");
            }
          }
        }
        catch (Exception CriticalEx)
        {
          Logger.Error($"[Web Server CRITICAL]: Could not run dummy server:\n{CriticalEx}");
        }
      });
    }
  }
}
