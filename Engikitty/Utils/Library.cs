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
            "we are so back",
            "let him cook",
            "real",
            "it is so over",
            "NEVER let them cook again",
            "bro is DELUSIONAL",
            "believe it or not you're going straight to jail",
            "no i dont think so",
            "understandable have a great day",
            "sure grandma lets get you to bed",
            "yes",
            "no",
            "yes my love",
            "no you fucking whore",
            "maya E, maya O, maya A",
            "idk ask that guy next door",
            "The Leading Ladies of Entertainment is an annual event organized by the Latin Recording Academy, the body that also distributes the Latin Grammy Awards, at which awards are presented to women excelling in the arts and sciences, and who have made indelible impressions and contributions to the Latin entertainment industry.",
            "ABSOLUTELY NOT",
            "absolutely my dear",
            "whatever you say",
            "the amazing digital footprint",
            "don't look behind you",
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
    }
}