using Npgsql;
using System.Text.Json;

namespace Engikitty.Services
{
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
        
        public static async Task WriteAsync(string Key, object Value)
        {
            var JsonString = JsonSerializer.Serialize(Value);
            var Query = "INSERT INTO BotStorage (Key, Value) VALUES (@Key, @Value) ON CONFLICT (Key) DO UPDATE SET Value = @Value;";
            
            await using var Command = DataSource.CreateCommand(Query);

            Command.Parameters.AddWithValue("Key", Key);
            Command.Parameters.AddWithValue("Value", JsonString);

            await Command.ExecuteNonQueryAsync();
        }

        public static async Task<T?> ReadAsync<T>(string Key)
        {
            var Query = "SELECT Value FROM BotStorage WHERE Key = @Key;";
            
            await using var Command = DataSource.CreateCommand(Query);

            Command.Parameters.AddWithValue("Key", Key);

            await using var Reader = await Command.ExecuteReaderAsync();
            if (await Reader.ReadAsync())
            {
                var JsonString = Reader.GetString(0);
                return JsonSerializer.Deserialize<T>(JsonString);
            }

            return default;
        }
    }
}