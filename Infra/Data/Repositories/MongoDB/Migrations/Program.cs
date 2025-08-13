using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 Iniciando Migrações MongoDB...");

        // 1️⃣ Lendo o appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connString = config["Migrations:projetoRl:ConnectionString"];
        if (string.IsNullOrEmpty(connString))
        {
            Console.WriteLine("❌ ConnectionString não encontrada no appsettings.json");
            return;
        }

        // 2️⃣ Conexão com MongoDB
        var client = new MongoClient(connString);
        var dbName = "projetoRl"; // Se precisar, pode colocar no appsettings também
        var database = client.GetDatabase(dbName);

        // Coleção de controle de migrações
        var migrationCollection = database.GetCollection<MigrationHistory>("_migrations");

        // 3️⃣ Procurar scripts na pasta Migrations
        var migrationsDir = Path.Combine(Directory.GetCurrentDirectory(), "migrations");
        if (!Directory.Exists(migrationsDir))
        {
            Console.WriteLine("❌ Pasta migrations não encontrada.");
            return;
        }

        var scriptFiles = Directory.GetFiles(migrationsDir, "*.js")
                                   .OrderBy(f => f) // Ordem alfabética
                                   .ToList();

        foreach (var scriptPath in scriptFiles)
        {
            var scriptName = Path.GetFileName(scriptPath);

            // Verificar se já foi executado
            var alreadyRan = await migrationCollection
                .Find(m => m.ScriptName == scriptName)
                .AnyAsync();

            if (alreadyRan)
            {
                Console.WriteLine($"⏩ {scriptName} já executado, pulando...");
                continue;
            }

            Console.WriteLine($"▶️ Executando migração: {scriptName}");

            try
            {
                // 4️⃣ Rodando script no Mongo
                var scriptContent = File.ReadAllText(scriptPath);

                // Executar como comando JavaScript
                var command = new BsonDocument
                {
                    { "eval", scriptContent }
                };

                await database.RunCommandAsync<BsonDocument>(command);

                // 5️⃣ Salvar no histórico
                await migrationCollection.InsertOneAsync(new MigrationHistory
                {
                    ScriptName = scriptName,
                    ExecutedAt = DateTime.UtcNow
                });

                Console.WriteLine($"✅ Migração {scriptName} concluída!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao executar {scriptName}: {ex.Message}");
                break; // Para a execução para evitar inconsistências
            }
        }

        Console.WriteLine("🏁 Todas as migrações processadas.");
    }
}

// Classe para controle de histórico
public class MigrationHistory
{
    public string ScriptName { get; set; }
    public DateTime ExecutedAt { get; set; }
}
