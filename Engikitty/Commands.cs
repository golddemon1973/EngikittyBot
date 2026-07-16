using System.Text;
using System.Text.Json;
using Engikitty.Commands;
using NetCord;
using NetCord.Services.ApplicationCommands;
using NetCord.Rest;

namespace Engikitty.Commands
{
    public class CommandInfo
    {
        public readonly bool IsEphemeral;
        public readonly bool IsHeavy;

        public CommandInfo(bool IsEphemeral = false, bool IsHeavy = false)
        {
            this.IsEphemeral = IsEphemeral;
            this.IsHeavy = IsHeavy;
        }
    }

    file static class CommandUtilities
    {
        #region BadTranslate

        private static readonly string[] LanguagePool =
        [
            "ace", "ady", "af", "ak", "am", "an", "ar", "as", "av", "awa",
            "ay", "az", "ba", "bal", "ban", "bar", "bcl", "be", "bem", "ber",
            "bg", "bho", "bi", "bjn", "bm", "bn", "bo", "br", "bs", "btv",
            "bua", "bug", "ca", "ceb", "ch", "chk", "cho", "chr", "chy", "co",
            "crh", "crs", "cs", "csb", "cu", "cv", "cy", "da", "de", "din",
            "doi", "dov", "dv", "dz", "ee", "el", "en", "eo", "es", "et",
            "eu", "fa", "ff", "fi", "fj", "fo", "fr", "frr", "fy", "ga",
            "gaa", "gd", "gl", "gn", "gom", "gor", "guc", "gu", "gv", "ha",
            "hak", "haw", "he", "hi", "hil", "hmn", "hop", "hr", "ht", "hu",
            "hy", "hz", "ia", "id", "ie", "ig", "ii", "ik", "ilo", "is",
            "it", "iu", "ja", "jam", "jv", "ka", "kaa", "kab", "kam", "kbd",
            "kbp", "kea", "kg", "khg", "ki", "kj", "kk", "kl", "km", "kn",
            "ko", "koi", "kr", "krc", "kri", "krj", "krl", "ks", "ksh", "ku",
            "kum", "kv", "kw", "ky", "la", "lb", "lbe", "lg", "li", "lij",
            "lim", "lmo", "ln", "lo", "loz", "lt", "ltg", "lu", "lua", "luo",
            "lus", "lv", "mad", "mag", "mai", "mak", "mas", "mdf", "mg", "mh",
            "mi", "min", "mk", "ml", "mn", "mni", "mnw", "mo", "mos", "mr",
            "mri", "ms", "mt", "mua", "mus", "mwl", "my", "myv", "mzn", "na",
            "nap", "nd", "ne", "new", "ng", "nia", "niu", "nl", "nn", "no",
            "nov", "nqo", "nr", "nso", "nus", "nv", "ny", "oc", "om", "or",
            "os", "pa", "pa-Arab", "pag", "pam", "pap", "pau", "pcm", "pl", "pms",
            "pnb", "pnt", "ps", "pt", "pt-BR", "pt-PT", "qu", "rcf", "rm", "rn",
            "ro", "rom", "ru", "rup", "rw", "sa", "sah", "sat", "sc", "scn",
            "sco", "sd", "se", "sg", "shn", "si", "sk", "sl", "sm", "sma",
            "sme", "smj", "smn", "sms", "sn", "so", "sq", "sr", "srn", "ss",
            "st", "su", "sv", "sw", "syr", "szl", "ta", "tcy", "te", "tet",
            "tg", "th", "ti", "tk", "tl", "tly", "tn", "to", "tok", "tpi",
            "tr", "ts", "tt", "tum", "tvl", "tw", "ty", "tyv", "tzm", "udm",
            "ug", "uk", "umb", "ur", "uz", "ve", "vec", "vi", "vls", "vmw",
            "vo", "vro", "wa", "war", "wen", "wo", "xal", "xh", "xmf", "yi",
            "yo", "yua", "za", "ze", "zh-CN", "zh-TW", "zu"
        ];

        private static readonly Dictionary<string, string> LanguageNamePool =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "ace", "Achinese" },
                { "ady", "Adyghe" },
                { "af", "Afrikaans" },
                { "ak", "Akan" },
                { "am", "Amharic" },
                { "an", "Aragonese" },
                { "ar", "Arabic" },
                { "as", "Assamese" },
                { "av", "Avaric" },
                { "awa", "Awadhi" },
                { "ay", "Aymara" },
                { "az", "Azerbaijani" },
                { "ba", "Bashkir" },
                { "bal", "Baluchi" },
                { "ban", "Balinese" },
                { "bar", "Bavarian" },
                { "bcl", "Central Bikol" },
                { "be", "Belarusian" },
                { "bem", "Bemba" },
                { "ber", "Berber" },
                { "bg", "Bulgarian" },
                { "bho", "Bhojpuri" },
                { "bi", "Bislama" },
                { "bjn", "Banjar" },
                { "bm", "Bambara" },
                { "bn", "Bengali" },
                { "bo", "Tibetan" },
                { "br", "Breton" },
                { "bs", "Bosnian" },
                { "btv", "Bateri" },
                { "bua", "Buriat" },
                { "bug", "Buginese" },
                { "ca", "Catalan" },
                { "ceb", "Cebuano" },
                { "ch", "Chamorro" },
                { "chk", "Chuukese" },
                { "cho", "Choctaw" },
                { "chr", "Cherokee" },
                { "chy", "Cheyenne" },
                { "co", "Corsican" },
                { "crh", "Crimean Tatar" },
                { "crs", "Seychelles Creole" },
                { "cs", "Czech" },
                { "csb", "Kashubian" },
                { "cu", "Church Slavic" },
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
                { "frr", "Northern Frisian" },
                { "fy", "Western Frisian" },
                { "ga", "Irish" },
                { "gaa", "Ga" },
                { "gd", "Scottish Gaelic" },
                { "gl", "Galician" },
                { "gn", "Guarani" },
                { "gom", "Goan Konkani" },
                { "gor", "Gorontalo" },
                { "guc", "Wayuu" },
                { "gu", "Gujarati" },
                { "gv", "Manx" },
                { "ha", "Hausa" },
                { "hak", "Hakka Chinese" },
                { "haw", "Hawaiian" },
                { "he", "Hebrew" },
                { "hi", "Hindi" },
                { "hil", "Hiligaynon" },
                { "hmn", "Hmong" },
                { "hop", "Hopi" },
                { "hr", "Croatian" },
                { "ht", "Haitian" },
                { "hu", "Hungarian" },
                { "hy", "Armenian" },
                { "hz", "Herero" },
                { "ia", "Interlingua" },
                { "id", "Indonesian" },
                { "ie", "Interlingue" },
                { "ig", "Igbo" },
                { "ii", "Sichuan Yi" },
                { "ik", "Inupiaq" },
                { "ilo", "Iloko" },
                { "is", "Icelandic" },
                { "it", "Italian" },
                { "iu", "Inuktitut" },
                { "ja", "Japanese" },
                { "jam", "Jamaican Patois" },
                { "jv", "Javanese" },
                { "ka", "Georgian" },
                { "kaa", "Kara-Kalpak" },
                { "kab", "Kabyle" },
                { "kam", "Kamba" },
                { "kbd", "Kabardian" },
                { "kbp", "Kabiye" },
                { "kea", "Kabuverdianu" },
                { "kg", "Kongo" },
                { "khg", "Khetrani" },
                { "ki", "Kikuyu" },
                { "kj", "Kuanyama" },
                { "kk", "Kazakh" },
                { "kl", "Kalaallisut" },
                { "km", "Central Khmer" },
                { "kn", "Kannada" },
                { "ko", "Korean" },
                { "koi", "Komi-Permyak" },
                { "kr", "Kanuri" },
                { "krc", "Karachay-Balkar" },
                { "kri", "Krio" },
                { "krj", "Kinaray-A" },
                { "krl", "Karelian" },
                { "ks", "Kashmiri" },
                { "ksh", "Colognian" },
                { "ku", "Kurdish" },
                { "kum", "Kumyk" },
                { "kv", "Komi" },
                { "kw", "Cornish" },
                { "ky", "Kirghiz" },
                { "la", "Latin" },
                { "lb", "Luxembourgish" },
                { "lbe", "Lak" },
                { "lg", "Ganda" },
                { "li", "Limburgan" },
                { "lij", "Ligurian" },
                { "lim", "Limburgish" },
                { "lmo", "Lombard" },
                { "ln", "Lingala" },
                { "lo", "Lao" },
                { "loz", "Lozi" },
                { "lt", "Lithuanian" },
                { "ltg", "Latgalian" },
                { "lu", "Luba-Katanga" },
                { "lua", "Luba-Lulua" },
                { "luo", "Luo" },
                { "lus", "Lushai" },
                { "lv", "Latvian" },
                { "mad", "Madurese" },
                { "mag", "Magahi" },
                { "mai", "Maithili" },
                { "mak", "Makasar" },
                { "mas", "Masai" },
                { "mdf", "Moksha" },
                { "mg", "Malagasy" },
                { "mh", "Marshallese" },
                { "mi", "Maori" },
                { "min", "Minangkabau" },
                { "mk", "Macedonian" },
                { "ml", "Malayalam" },
                { "mn", "Mongolian" },
                { "mni", "Manipuri" },
                { "mnw", "Mon" },
                { "mo", "Moldavian" },
                { "mos", "Mossi" },
                { "mr", "Marathi" },
                { "mri", "Maori" },
                { "ms", "Malay" },
                { "mt", "Maltese" },
                { "mua", "Mundang" },
                { "mus", "Creek" },
                { "mwl", "Mirandese" },
                { "my", "Burmese" },
                { "myv", "Erzya" },
                { "mzn", "Mazanderani" },
                { "na", "Nauru" },
                { "nap", "Neapolitan" },
                { "nd", "North Ndebele" },
                { "ne", "Nepali" },
                { "new", "Newari" },
                { "ng", "Ndonga" },
                { "nia", "Nias" },
                { "niu", "Niuean" },
                { "nl", "Dutch" },
                { "nn", "Norwegian Nynorsk" },
                { "no", "Norwegian" },
                { "nov", "Novial" },
                { "nqo", "N'Ko" },
                { "nr", "South Ndebele" },
                { "nso", "Pedi" },
                { "nus", "Nuer" },
                { "nv", "Navajo" },
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
                { "pau", "Palauan" },
                { "pcm", "Nigerian Pidgin" },
                { "pl", "Polish" },
                { "pms", "Piedmontese" },
                { "pnb", "Western Panjabi" },
                { "pnt", "Pontic" },
                { "ps", "Pushto" },
                { "pt", "Portuguese" },
                { "pt-BR", "Portuguese (Brazil)" },
                { "pt-PT", "Portuguese (Portugal)" },
                { "qu", "Quechua" },
                { "rcf", "Reunion Creole" },
                { "rm", "Romansh" },
                { "rn", "Rundi" },
                { "ro", "Romanian" },
                { "rom", "Romany" },
                { "ru", "Russian" },
                { "rup", "Aromanian" },
                { "rw", "Kinyarwanda" },
                { "sa", "Sanskrit" },
                { "sah", "Yakut" },
                { "sat", "Santali" },
                { "sc", "Sardinian" },
                { "scn", "Sicilian" },
                { "sco", "Scots" },
                { "sd", "Sindhi" },
                { "se", "Northern Sami" },
                { "sg", "Sango" },
                { "shn", "Shan" },
                { "si", "Sinhala" },
                { "sk", "Slovak" },
                { "sl", "Slovenian" },
                { "sm", "Samoan" },
                { "sma", "Southern Sami" },
                { "sme", "Northern Sami" },
                { "smj", "Lule Sami" },
                { "smn", "Inari Sami" },
                { "sms", "Skolt Sami" },
                { "sn", "Shona" },
                { "so", "Somali" },
                { "sq", "Albanian" },
                { "sr", "Serbian" },
                { "srn", "Sranan Tongo" },
                { "ss", "Swati" },
                { "st", "Southern Sotho" },
                { "su", "Sundanese" },
                { "sv", "Swedish" },
                { "sw", "Swahili" },
                { "syr", "Syriac" },
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
                { "tly", "Talysh" },
                { "tn", "Tswana" },
                { "to", "Tonga" },
                { "tok", "Toki Pona" },
                { "tpi", "Tok Pisin" },
                { "tr", "Turkish" },
                { "ts", "Tsonga" },
                { "tt", "Tatar" },
                { "tum", "Tumbuka" },
                { "tvl", "Tuvaluan" },
                { "tw", "Twi" },
                { "ty", "Tahitian" },
                { "tyv", "Tuvinian" },
                { "tzm", "Central Atlas Tamazight" },
                { "udm", "Udmurt" },
                { "ug", "Uighur" },
                { "uk", "Ukrainian" },
                { "umb", "Umbundu" },
                { "ur", "Urdu" },
                { "uz", "Uzbek" },
                { "ve", "Venda" },
                { "vec", "Venetian" },
                { "vi", "Vietnamese" },
                { "vls", "West Flemish" },
                { "vmw", "Makhuwa" },
                { "vo", "Volapük" },
                { "vro", "Võro" },
                { "wa", "Walloon" },
                { "war", "Waray" },
                { "wen", "Sorbian" },
                { "wo", "Wolof" },
                { "xal", "Kalmyk" },
                { "xh", "Xhosa" },
                { "xmf", "Mingrelian" },
                { "yi", "Yiddish" },
                { "yo", "Yoruba" },
                { "yua", "Yucatec Maya" },
                { "za", "Zhuang" },
                { "ze", "Zeelandic" },
                { "zh-CN", "Chinese (Simplified)" },
                { "zh-TW", "Chinese (Traditional)" },
                { "zu", "Zulu" }
            };

        private static readonly HttpClient TranslateClient = new();

        public static async Task DoBadTranslate(string Text, int Times, IApplicationCommandContext Context)
        {
            Dictionary<string, string> BadTranslated = await CommandUtilities.BadTranslate(Text, Times);

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
            Dictionary<string, string> BadTranslated = await CommandUtilities.BadTranslate(Text, Times);

            await Context.Interaction.ModifyResponseAsync(Message =>
            {
                Message.Embeds =
                [
                    new EmbedProperties()
                    {
                        Thumbnail = new EmbedThumbnailProperties(
                            "https://cdn.discordapp.com/attachments/1505301024443994263/1526178240568229958/bleh.jpg?ex=6a5613bf&is=6a54c23f&hm=ea363ec0295c9090ccdefbafa73d3a015b4a54ece56661665750e21e4bd5ea3b&"),
                        Title = "Done!!",
                        Description = "Engikitty hit the text, but not that hard. It'll likely be fine.",
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

    [SlashCommand("bot", "All sorts of tools regarding engikitty itself",
        Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel])]
    public class BotModule : ApplicationCommandModule<ApplicationCommandContext>
    {
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

    [SlashCommand("fun", "Alot of stuff for fun, probably?",
        Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel])]
    public class FunModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        [SubSlashCommand("badtranslate", "Translate something. But like a lot.")]
        public async Task BadTranslate(
            [SlashCommandParameter(Name = "text", Description = "The text to translate", MaxLength = 768)]
            string Text,
            [SlashCommandParameter(Name = "loops", Description = "Times to loop", MinValue = 1, MaxValue = 100)]
            int Times = 5)
        {
            await CommandUtilities.DoBadTranslate(Text, Times, Context);
        }
    }

    public class ContextModule : ApplicationCommandModule<ApplicationCommandContext>
    {
        [MessageCommand("Bad Translate (5 times)",
            Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel],
            IntegrationTypes = [ApplicationIntegrationType.GuildInstall, ApplicationIntegrationType.UserInstall])]
        public async Task BadTranslate5(RestMessage Msg)
        {
            await CommandUtilities.DoMessageBadTranslate(Msg.Content, 5, Context);
        }

        [MessageCommand("Bad Translate (10 times)",
            Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel],
            IntegrationTypes = [ApplicationIntegrationType.GuildInstall, ApplicationIntegrationType.UserInstall])]
        public async Task BadTranslate10(RestMessage Msg)
        {
            await CommandUtilities.DoMessageBadTranslate(Msg.Content, 20, Context);
        }

        [MessageCommand("Bad Translate (20 times)",
            Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel],
            IntegrationTypes = [ApplicationIntegrationType.GuildInstall, ApplicationIntegrationType.UserInstall])]
        public async Task BadTranslate20(RestMessage Msg)
        {
            await CommandUtilities.DoMessageBadTranslate(Msg.Content, 20, Context);
        }
    }
}

namespace Engikitty.Bot
{
    public static class Info
    {
        public static readonly Dictionary<string, CommandInfo> Commands = new()
        {
            ["bot ping"] = new(),
            ["fun badtranslate"] = new(false, true),
            ["Bad Translate (5 times)"] = new(false, true),
            ["Bad Translate (10 times)"] = new(false, true),
            ["Bad Translate (20 times)"] = new(false, true),
        };
    }
}