/*

  Code is property of @youraveragekitty on Discord.

  Redistribution that does not follow the "BSD 3-Clause" License protecting the EngikittyBot project is not allowed.

*/

using Npgsql;
using System.Text.Json;

namespace Engikitty.Services
{
    /// <summary>
    /// Service regarding the Neon Postgre database
    /// </summary>
    public static class Database
    {
        private static readonly string ConnectionString =
            "Host=ep-nameless-lake-asii0z56.c-4.eu-central-1.aws.neon.tech;" +
            "Port=5432;" +
            "Database=neondb;" +
            "Username=neondb_owner;" +
            $"Password={Environment.GetEnvironmentVariable("DISCORD_BOT_DATABASE_TOKEN_ENGIKITTY")};" +
            "SSL Mode=Require;" +
            "Trust Server Certificate=true;" +
            "Gss Encryption Mode=Disable;";

        private static readonly NpgsqlDataSource DataSource = NpgsqlDataSource.Create(ConnectionString);

        /// <summary>
        /// Write a value
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static async Task WriteAsync(string Key, object Value)
        {
            string JsonString = JsonSerializer.Serialize(Value);
            string Query =
                "INSERT INTO BotStorage (Key, Value) VALUES (@Key, @Value) ON CONFLICT (Key) DO UPDATE SET Value = @Value;";

            await using NpgsqlCommand Command = DataSource.CreateCommand(Query);

            Command.Parameters.AddWithValue("Key", Key);
            Command.Parameters.AddWithValue("Value", JsonString);

            await Command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Read a value
        /// </summary>
        /// <param name="Key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T?> ReadAsync<T>(string Key)
        {
            string Query = "SELECT Value FROM BotStorage WHERE Key = @Key;";

            await using NpgsqlCommand Command = DataSource.CreateCommand(Query);

            Command.Parameters.AddWithValue("Key", Key);

            await using NpgsqlDataReader Reader = await Command.ExecuteReaderAsync();
            if (await Reader.ReadAsync())
            {
                string JsonString = Reader.GetString(0);
                return JsonSerializer.Deserialize<T>(JsonString);
            }

            return default;
        }
    }
}