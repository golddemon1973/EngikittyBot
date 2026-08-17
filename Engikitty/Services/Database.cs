/*

  Code is property of @youraveragekitty on Discord.

  Redistribution that does not follow the "BSD 3-Clause" License protecting the EngikittyBot project is not allowed.

*/

using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace Engikitty.Services
{
    /// <summary>
    /// Service regarding the local SQLite database
    /// </summary>
    public static class Database
    {
        private static readonly string ConnectionString = $"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "engikitty.db")};";

        private static bool Initialized = false;
        private static readonly SemaphoreSlim InitLock = new(1, 1);

        /// <summary>
        /// Ensures the BotStorage table exists before any read/write operation.
        /// </summary>
        private static async Task EnsureInitializedAsync()
        {
            if (Initialized)
            {
                return;
            }

            await InitLock.WaitAsync();
            try
            {
                if (Initialized)
                {
                    return;
                }

                await using SqliteConnection Connection = new(ConnectionString);
                await Connection.OpenAsync();

                string Query =
                    "CREATE TABLE IF NOT EXISTS BotStorage (Key TEXT PRIMARY KEY, Value TEXT NOT NULL);";

                await using SqliteCommand Command = Connection.CreateCommand();
                Command.CommandText = Query;
                await Command.ExecuteNonQueryAsync();

                Initialized = true;
            }
            finally
            {
                InitLock.Release();
            }
        }

        /// <summary>
        /// Write a value
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static async Task WriteAsync(string Key, object Value)
        {
            await EnsureInitializedAsync();

            string JsonString = JsonSerializer.Serialize(Value);
            string Query =
                "INSERT INTO BotStorage (Key, Value) VALUES (@Key, @Value) " +
                "ON CONFLICT (Key) DO UPDATE SET Value = @Value;";

            await using SqliteConnection Connection = new(ConnectionString);
            await Connection.OpenAsync();

            await using SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = Query;
            Command.Parameters.AddWithValue("@Key", Key);
            Command.Parameters.AddWithValue("@Value", JsonString);

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
            await EnsureInitializedAsync();

            string Query = "SELECT Value FROM BotStorage WHERE Key = @Key;";

            await using SqliteConnection Connection = new(ConnectionString);
            await Connection.OpenAsync();

            await using SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = Query;
            Command.Parameters.AddWithValue("@Key", Key);

            await using SqliteDataReader Reader = await Command.ExecuteReaderAsync();
            if (await Reader.ReadAsync())
            {
                string JsonString = Reader.GetString(0);
                return JsonSerializer.Deserialize<T>(JsonString);
            }

            return default;
        }
    }
}