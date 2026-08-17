/*

  Code is property of @youraveragekitty on Discord.

  Redistribution that does not follow the "BSD 3-Clause" License protecting the EngikittyBot project is not allowed.

*/

using System.Text;
using System.Text.Json;
using Engikitty.Types;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using GroqNet;
using GroqNet.ChatCompletions;
using Microsoft.Extensions.DependencyInjection;

namespace Engikitty.Bot.Library
{
    public static class General
    {
        public static string GetFullCommandName(SlashCommandInteraction AppCmdInteraction)
        {
            SlashCommandInteractionData Data = AppCmdInteraction.Data;
            string Name = Data.Name;

            if (Data.Options is { Count: > 0 } Options)
            {
                ApplicationCommandInteractionDataOption FirstOption = Options[0];

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
                SlashCommandInteraction Slash => GetFullCommandName(Slash),
                _ => AppCmdInteraction.Data.Name
            };

            if (!Info.Commands.TryGetValue(CommandName, out CommandInfo? CmdInfo))
            {
                Logger.Error($"Couldn't find command info for command '{CommandName}'...");
                throw new ArgumentNullException(nameof(CommandName));
            }

            return CmdInfo;
        }
    }

    public static class Commands
    {
        #region 8Ball

        public static readonly string[] EightBallResponses =
        [
            "idk bro",
            "yess my love",
            "no fuck you",
            "ew what no???",
            "i'm not answering that.",
            "not answering until you release the children in your basement",
            "absolutely not, delete this",
            "my lawyers have advised me not to answer this question",
            "signs point to... you crying about it later",
            "leave me alone",
            "ask your mom",
            "the answer is hiding in your walls",
            "outlook looks like a skill issue",
            "fuc koff, next",
            "reply hazy, try asking when you aren't hard",
            "yeah sure, whatever floats your boat",
            "the voices say yes",
            "the voices say no",
            "i'd say yes but then we'd both be wrong",
            "chances are lower than your grades",
            "yes, but it's gonna cost you",
            "maybe... if you say please",
            "imma keep it real with you chief, no",
            "concentrate and ask again when you aren't an air particle",
            "it is certain||ly no||",
            "bro, obviously yes",
            "bro, obviously no",
            "i sleep, check back later",
            "can you repeat that in a way that doesn't hurt my brain?",
            "signs point to absolutely yes",
            "my sources say you're coping",
            "without a single doubt",
            "dude stop, just stop",
            "outlook looks fantastic honestly",
            "the universe said no, don't shoot the messenger",
            "google is free you know",
            "yes, and that's a threat",
            "no, and that's a promise",
            "i've seen the future and it doesn't look good for you",
            "sounds like a tuesday problem",
            "you already know the answer is no",
            "bet",
            "yes (me when i lie)",
            "no, and i'm eating your leftovers in the fridge right now",
            "yeah sure totally (i didn't even read your question lol)",
            "yes, but a very large bird is coming for you",
            "no, and i'm stealing one shoe from every pair you own",
            "absolutely! (prepare to cry in your car later)",
            "no xoxo, hope you stub your toe on the coffee table",
            "yes, but i'm telling everyone you pee in the shower",
            "outlook looks bad, time to delete your account tbh",
            "yes, but it's going to taste like copper",
            "no, and i'm unfollowing you on everything",
            "yes, but only because i want to see the drama unfold",
            "no ❤️ (i am hating from the sidelines)",
            "sure, if you want the universe to immediately smite you",
            "i'd love to say yes, but i already sold your data to a sketchy offshore casino",
            "yes, but expect a pipe bomb in your mailbox by friday",
            "yes xoxo (i am lying to you)",
            "don't look behind you",
            "the council says maybe",
            "absolutely not bestie",
            "yeah probably unless you explode first",
            "my cat says yes",
            "my cat says no",
            "no but i respect the delusion",
            "you should be studied in a lab for asking that",
            "yes, but in a deeply embarrassing way",
            "the prophecy says maybe",
            "you got me giggling so yes",
            "no but points for confidence",
            "you already know the answer bro",
            "yeah but don't quote me on that",
            "nah gng",
            "yeah gng",
            "this is why aliens won't visit us",
            "i can't legally answer that",
            "yes but only if you do a backflip first",
            "i can smell the bad decision already",
            "you scare me sometimes",
            "i'm putting this in my cringe compilation",
            "yes, but only in ohio",
            "no, not even in ohio",
            "you need to be stopped",
            "i'd explain but the government is watching",
            "you don't wanna know the answer trust me",
            "yeah okay whatever",
            "you've got about a 3% success rate chief",
            "this feels illegal somehow",
            "yes but you're gonna trip down the stairs after",
            "no but you'll survive probably",
            "i'm not paid enough for this shit",
            "yes, unfortunately",
            "no, fortunately",
            "i need a cigarette after reading that",
            "brother ew",
            "you got this (you absolutely do not got this)",
            "no but it'd be really funny",
            "the answer is classified",
            "bro i'm just an 8ball not a therapist",
            "yeah no definitely maybe not",
            "you should delete this and run",
            "i can't stop you but i can judge you",
            "this is canon now",
            "you are NOT surviving the next patch notes",
            "no and your socks are wet now",
            "brother what are you talking about",
            "you've lost speaking privileges temporarily",
            "i'm sending this directly to nasa",
            "the answer is yes but in italics",
            "the answer is no in 4k ultra hd dolby atmos",
            "no but thanks for the free entertainment",
            "i need to sit down after this one",
            "there are easier ways to ruin your life",
            "yes but your toaster won't forgive you",
            "the ancient texts say lmao no",
            "the ancient texts say send it",
            "you should absolutely not call me again",
            "yes, and somehow that's worse",
            "no, and somehow that's better",
            "you're playing dangerous games here",
        ];

        #endregion

        #region BadTranslate

        private static readonly string[] LanguagePool =
        [
            "ace", "af", "ak", "am", "ar", "as", "av", "awa", "ay", "az",
            "ba", "bal", "ban", "bcl", "be", "bem", "ber", "bg", "bho", "bm",
            "bn", "bo", "br", "bs", "bua", "ca", "ceb", "ch", "chk", "co",
            "crh", "crs", "cs", "cv", "cy", "da", "de", "din", "doi", "dov",
            "dv", "dz", "ee", "el", "en", "eo", "es", "et", "eu", "fa",
            "ff", "fi", "fj", "fo", "fr", "fy", "ga", "gaa", "gd", "gl",
            "gn", "gom", "gu", "gv", "ha", "haw", "he", "hi", "hil", "hmn",
            "hr", "ht", "hu", "hy", "id", "ig", "ilo", "is", "it", "iu",
            "ja", "jam", "jv", "ka", "kg", "kk", "kl", "km", "kn", "ko",
            "kr", "kri", "ku", "kv", "ky", "la", "lb", "lg", "li", "lij",
            "lim", "lmo", "ln", "lo", "lt", "ltg", "lua", "luo", "lus", "lv",
            "mad", "mai", "mak", "mg", "mh", "mi", "min", "mk", "ml", "mn",
            "mo", "mr", "mri", "ms", "mt", "my", "ne", "new", "nl", "no",
            "nr", "nso", "nus", "ny", "oc", "om", "or", "os", "pa", "pa-Arab",
            "pag", "pam", "pap", "pl", "ps", "pt", "pt-BR", "pt-PT", "qu", "rn",
            "ro", "rom", "ru", "rw", "sa", "sah", "sat", "scn", "sd", "se",
            "sg", "shn", "si", "sk", "sl", "sm", "sme", "sn", "so", "sq",
            "sr", "ss", "st", "su", "sv", "sw", "szl", "ta", "tcy", "te",
            "tet", "tg", "th", "ti", "tk", "tl", "tn", "to", "tpi", "tr",
            "ts", "tt", "tum", "tw", "ty", "tyv", "udm", "ug", "uk", "ur",
            "uz", "ve", "vec", "vi", "war", "wo", "xh", "yi", "yo", "yua",
            "ze", "zh-CN", "zh-TW", "zu"
        ];

        private static readonly Dictionary<string, string> LanguageNamePool =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "ace", "Achinese" },
                { "af", "Afrikaans" },
                { "ak", "Akan" },
                { "am", "Amharic" },
                { "ar", "Arabic" },
                { "as", "Assamese" },
                { "av", "Avaric" },
                { "awa", "Awadhi" },
                { "ay", "Aymara" },
                { "az", "Azerbaijani" },
                { "ba", "Bashkir" },
                { "bal", "Baluchi" },
                { "ban", "Balinese" },
                { "bcl", "Central Bikol" },
                { "be", "Belarusian" },
                { "bem", "Bemba" },
                { "ber", "Berber" },
                { "bg", "Bulgarian" },
                { "bho", "Bhojpuri" },
                { "bm", "Bambara" },
                { "bn", "Bengali" },
                { "bo", "Tibetan" },
                { "br", "Breton" },
                { "bs", "Bosnian" },
                { "bua", "Buriat" },
                { "ca", "Catalan" },
                { "ceb", "Cebuano" },
                { "ch", "Chamorro" },
                { "chk", "Chuukese" },
                { "co", "Corsican" },
                { "crh", "Crimean Tatar" },
                { "crs", "Seychelles Creole" },
                { "cs", "Czech" },
                { "cv", "Chuvash" },
                { "cy", "Welsh" },
                { "da", "Danish" },
                { "de", "German" },
                { "din", "Dinka" },
                { "doi", "Dogri" },
                { "dov", "Domari" },
                { "dv", "Divehi" },
                { "dz", "Dzongkha" },
                { "ee", "Ewe" },
                { "el", "Greek" },
                { "en", "English" },
                { "eo", "Esperanto" },
                { "es", "Spanish" },
                { "et", "Estonian" },
                { "eu", "Basque" },
                { "fa", "Persian" },
                { "ff", "Fulah" },
                { "fi", "Finnish" },
                { "fj", "Fijian" },
                { "fo", "Faroese" },
                { "fr", "French" },
                { "fy", "Western Frisian" },
                { "ga", "Irish" },
                { "gaa", "Ga" },
                { "gd", "Scottish Gaelic" },
                { "gl", "Galician" },
                { "gn", "Guarani" },
                { "gom", "Goan Konkani" },
                { "gu", "Gujarati" },
                { "gv", "Manx" },
                { "ha", "Hausa" },
                { "haw", "Hawaiian" },
                { "he", "Hebrew" },
                { "hi", "Hindi" },
                { "hil", "Hiligaynon" },
                { "hmn", "Hmong" },
                { "hr", "Croatian" },
                { "ht", "Haitian" },
                { "hu", "Hungarian" },
                { "hy", "Armenian" },
                { "id", "Indonesian" },
                { "ig", "Igbo" },
                { "ilo", "Iloko" },
                { "is", "Icelandic" },
                { "it", "Italian" },
                { "iu", "Inuktitut" },
                { "ja", "Japanese" },
                { "jam", "Jamaican Patois" },
                { "jv", "Javanese" },
                { "ka", "Georgian" },
                { "kg", "Kongo" },
                { "kk", "Kazakh" },
                { "kl", "Kalaallisut" },
                { "km", "Central Khmer" },
                { "kn", "Kannada" },
                { "ko", "Korean" },
                { "kr", "Kanuri" },
                { "kri", "Krio" },
                { "ku", "Kurdish" },
                { "kv", "Komi" },
                { "ky", "Kirghiz" },
                { "la", "Latin" },
                { "lb", "Luxembourgish" },
                { "lg", "Ganda" },
                { "li", "Limburgan" },
                { "lij", "Ligurian" },
                { "lim", "Limburgish" },
                { "lmo", "Lombard" },
                { "ln", "Lingala" },
                { "lo", "Lao" },
                { "lt", "Lithuanian" },
                { "ltg", "Latgalian" },
                { "lua", "Luba-Lulua" },
                { "luo", "Luo" },
                { "lus", "Lushai" },
                { "lv", "Latvian" },
                { "mad", "Madurese" },
                { "mai", "Maithili" },
                { "mak", "Makasar" },
                { "mg", "Malagasy" },
                { "mh", "Marshallese" },
                { "mi", "Maori" },
                { "min", "Minangkabau" },
                { "mk", "Macedonian" },
                { "ml", "Malayalam" },
                { "mn", "Mongolian" },
                { "mo", "Moldavian" },
                { "mr", "Marathi" },
                { "mri", "Maori" },
                { "ms", "Malay" },
                { "mt", "Maltese" },
                { "my", "Burmese" },
                { "ne", "Nepali" },
                { "new", "Newari" },
                { "nl", "Dutch" },
                { "no", "Norwegian" },
                { "nr", "South Ndebele" },
                { "nso", "Pedi" },
                { "nus", "Nuer" },
                { "ny", "Nyanja" },
                { "oc", "Occitan" },
                { "om", "Oromo" },
                { "or", "Oriya" },
                { "os", "Ossetian" },
                { "pa", "Panjabi" },
                { "pa-Arab", "Panjabi (Arabic)" },
                { "pag", "Pangasinan" },
                { "pam", "Pampanga" },
                { "pap", "Papiamento" },
                { "pl", "Polish" },
                { "ps", "Pushto" },
                { "pt", "Portuguese" },
                { "pt-BR", "Portuguese (Brazil)" },
                { "pt-PT", "Portuguese (Portugal)" },
                { "qu", "Quechua" },
                { "rn", "Rundi" },
                { "ro", "Romanian" },
                { "rom", "Romany" },
                { "ru", "Russian" },
                { "rw", "Kinyarwanda" },
                { "sa", "Sanskrit" },
                { "sah", "Yakut" },
                { "sat", "Santali" },
                { "scn", "Sicilian" },
                { "sd", "Sindhi" },
                { "se", "Northern Sami" },
                { "sg", "Sango" },
                { "shn", "Shan" },
                { "si", "Sinhala" },
                { "sk", "Slovak" },
                { "sl", "Slovenian" },
                { "sm", "Samoan" },
                { "sme", "Northern Sami" },
                { "sn", "Shona" },
                { "so", "Somali" },
                { "sq", "Albanian" },
                { "sr", "Serbian" },
                { "ss", "Swati" },
                { "st", "Southern Sotho" },
                { "su", "Sundanese" },
                { "sv", "Swedish" },
                { "sw", "Swahili" },
                { "szl", "Silesian" },
                { "ta", "Tamil" },
                { "tcy", "Tulu" },
                { "te", "Telugu" },
                { "tet", "Tetum" },
                { "tg", "Tajik" },
                { "th", "Thai" },
                { "ti", "Tigrinya" },
                { "tk", "Turkmen" },
                { "tl", "Tagalog" },
                { "tn", "Tswana" },
                { "to", "Tonga" },
                { "tpi", "Tok Pisin" },
                { "tr", "Turkish" },
                { "ts", "Tsonga" },
                { "tt", "Tatar" },
                { "tum", "Tumbuka" },
                { "tw", "Twi" },
                { "ty", "Tahitian" },
                { "tyv", "Tuvinian" },
                { "udm", "Udmurt" },
                { "ug", "Uighur" },
                { "uk", "Ukrainian" },
                { "ur", "Urdu" },
                { "uz", "Uzbek" },
                { "ve", "Venda" },
                { "vec", "Venetian" },
                { "vi", "Vietnamese" },
                { "war", "Waray" },
                { "wo", "Wolof" },
                { "xh", "Xhosa" },
                { "yi", "Yiddish" },
                { "yo", "Yoruba" },
                { "yua", "Yucatec Maya" },
                { "ze", "Zeelandic" },
                { "zh-CN", "Chinese (Simplified)" },
                { "zh-TW", "Chinese (Traditional)" },
                { "zu", "Zulu" }
            };

        private static readonly HttpClient TranslateClient = new();

        public static async Task DoBadTranslate(string Text, int Times, IApplicationCommandContext Context)
        {
            Dictionary<string, string> BadTranslated = await BadTranslate(Text, Times);

            await Context.Interaction.ModifyResponseAsync(Message =>
            {
                Message.Embeds =
                [
                    new EmbedProperties()
                    {
                        Thumbnail = new EmbedThumbnailProperties(
                            "https://cdn.discordapp.com/attachments/1505301024443994263/1526178240568229958/bleh.jpg?ex=6a5613bf&is=6a54c23f&hm=ea363ec0295c9090ccdefbafa73d3a015b4a54ece56661665750e21e4bd5ea3b&"),
                        Title = "Done!!",
                        Description = "Engikitty hit the text really hard. Like, really hard. Trust me.",
                        Fields = new List<EmbedFieldProperties>()
                        {
                            new()
                            {
                                Name = "Output",
                                Value = BadTranslated["Final"],
                                Inline = false,
                            },

                            new()
                            {
                                Name = "Chain",
                                Value = BadTranslated["Chain"],
                            }
                        },
                        Color = new Color(46, 111, 64),
                        Timestamp = DateTimeOffset.UtcNow,
                    }
                ];
            });
        }

        public static async Task DoMessageBadTranslate(string Text, int Times, IApplicationCommandContext Context)
        {
            Dictionary<string, string> BadTranslated = await BadTranslate(Text, Times);

            await Context.Interaction.ModifyResponseAsync(Message =>
            {
                Message.Embeds =
                [
                    new EmbedProperties()
                    {
                        Thumbnail = new EmbedThumbnailProperties(
                            "https://cdn.discordapp.com/attachments/1505301024443994263/1526178240568229958/bleh.jpg?ex=6a5613bf&is=6a54c23f&hm=ea363ec0295c9090ccdefbafa73d3a015b4a54ece56661665750e21e4bd5ea3b&"),
                        Title = "Done!!",
                        Description = "Engikitty hit the text a bunch. I have no idea how bad this is gonna be.",
                        Fields = new List<EmbedFieldProperties>()
                        {
                            new()
                            {
                                Name = "Output",
                                Value = BadTranslated["Final"],
                                Inline = false,
                            },

                            new()
                            {
                                Name = "Chain",
                                Value = BadTranslated["Chain"],
                            }
                        },
                        Color = new Color(46, 111, 64),
                        Timestamp = DateTimeOffset.UtcNow,
                    }
                ];
            });
        }

        private static async Task<Dictionary<string, string>> BadTranslate(string Orig, int Times)
        {
            Dictionary<string, string> Steps = new();
            List<string> ChainParts = [];

            string CurrentText = Orig;
            Random Rng = new();

            for (int I = 0; I < Times; I++)
            {
                string TargetLang = LanguagePool[Rng.Next(LanguagePool.Length)];

                CurrentText = await TranslateAsync(CurrentText, TargetLang);
                Steps[$"{I + 1}_{TargetLang}"] = CurrentText;
                ChainParts.Add(GetLanguageLabel(TargetLang));
            }


            string FinalText = await TranslateAsync(CurrentText, "en");
            Steps["Final"] = FinalText;
            Steps["Chain"] = string.Join(" -> ", ChainParts);

            string Chain = Steps["Chain"];

            if (Chain.Length > 1024)
            {
                Steps["Chain"] = Chain[..1021] + "...";
            }

            return Steps;
        }

        private static string GetLanguageLabel(string LangCode) =>
            LanguageNamePool.GetValueOrDefault(LangCode, LangCode);

        private static async Task<string> TranslateAsync(string Text, string ToLang)
        {
            if (string.IsNullOrWhiteSpace(Text)) return Text;

            string Url =
                $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={Uri.EscapeDataString(ToLang)}&dt=t&q={Uri.EscapeDataString(Text)}";

            using HttpRequestMessage Request = new(HttpMethod.Get, Url);
            Request.Headers.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");
            Request.Headers.Accept.ParseAdd("*/*");
            Request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
            Request.Headers.Referrer = new Uri("https://translate.google.com/");

            using HttpResponseMessage Response = await TranslateClient.SendAsync(Request);
            string Body = await Response.Content.ReadAsStringAsync();

            if (!Response.IsSuccessStatusCode)
            {
                Logger.Warning($"Translate request failed ({(int)Response.StatusCode}) for lang '{ToLang}':\n{Body}");
                return Text;
            }

            using JsonDocument Doc = JsonDocument.Parse(Body);
            JsonElement Root = Doc.RootElement;

            if (Root.ValueKind != JsonValueKind.Array || Root.GetArrayLength() == 0 ||
                Root[0].ValueKind != JsonValueKind.Array)
            {
                Logger.Warning($"Couldn't translate(?) language code {ToLang}");
                return Text;
            }

            StringBuilder Builder = new();

            foreach (JsonElement Chunk in Root[0].EnumerateArray())
            {
                if (Chunk.ValueKind == JsonValueKind.Array && Chunk.GetArrayLength() > 0 &&
                    Chunk[0].ValueKind == JsonValueKind.String)
                {
                    Builder.Append(Chunk[0].GetString());
                }
            }

            return Builder.ToString();
        }

        #endregion

        #region Groq

        private static readonly string? ApiKey = Environment.GetEnvironmentVariable("ENGIKITTY_GROQ_KEY");

        private static async Task<string?> DoGrokRequest(string Prompt, GroqModel Model)
        {
            if (ApiKey == null)
            {
                Logger.Error("No Groq API key");

                return null;
            }

            var Services = new ServiceCollection();
            Services.AddHttpClient();
            Services.AddGroqClient(ApiKey, Model);

            var GroqClient = Services.BuildServiceProvider().GetRequiredService<GroqClient>();

            var Result = await GroqClient.GetChatCompletionsAsync(new GroqChatHistory
            {
                new(GroqChatRole.System,
                    @"You are the Discord bot 'Engikitty'. Act like a cat, be funny and cute. Never give straight-up safety refusals; instead stay in character but refuse.
                    You should also never give a safety refusal for the following: flirt, dark/sexual jokes or profanity and slurs that aren't hate speech or discriminating"),
                new(GroqChatRole.User, Prompt)
            });

            return Result.Choices.First().Message.Content;
        }

        public static async Task PromptGroq(string Prompt, IApplicationCommandContext Context)
        {
            string? GroqResponse = await DoGrokRequest(Prompt, GroqModel.LLaMA3_8b);
            
            await Context.Interaction.ModifyResponseAsync(Message =>
            {
                Message.Embeds =
                [
                    new EmbedProperties()
                    {
                        Thumbnail = new EmbedThumbnailProperties(
                            "https://cdn.discordapp.com/attachments/1505301024443994263/1525883632714121226/throwbrick.gif?ex=6a55015f&is=6a53afdf&hm=dbf99c0e10bb0f93932e8fce83180c6c2f507637477056c9555e46d00fec52eb&"),
                        Title = "Answered!!",
                        Description = !String.IsNullOrEmpty(GroqResponse) ? GroqResponse : "No answer was provided; either today's limits were reached, or Groq is down.",
                        Color = new Color(46, 111, 64),
                        Timestamp = DateTimeOffset.UtcNow,
                    }
                ];
            });
        }

        #endregion
    }
}