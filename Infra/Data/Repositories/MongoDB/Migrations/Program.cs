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
        var dbName = "projetoRl"; // Pode colocar no appsettings se quiser flexibilidade

        if (string.IsNullOrEmpty(connString))
        {
            Console.WriteLine("❌ ConnectionString não encontrada no appsettings.json");
            return;
        }

        // 2️⃣ Conexão com MongoDB
        var client = new MongoClient(connString);
        var database = client.GetDatabase(dbName);

        // Coleção de controle de migrações
        var migrationCollection = database.GetCollection<MigrationHistory>("_migrations");

        // 3️⃣ Criar collections diretamente
        var collectionsToCreate = new[] { "user", "courier", "rental", "bike", "access_token" };

        foreach (var collName in collectionsToCreate)
        {
            // Verifica se a collection já existe
            var alreadyExists = (await database.ListCollectionNames().ToListAsync()).Contains(collName);
            if (!alreadyExists)
            {
                await database.CreateCollectionAsync(collName);
                Console.WriteLine($"✅ Collection {collName} criada!");

                // Registrar no histórico de migrações
                await migrationCollection.InsertOneAsync(new MigrationHistory
                {
                    ScriptName = $"CreateCollection_{collName}",
                    ExecutedAt = DateTime.UtcNow
                });
            }
            else
            {
                Console.WriteLine($"⏩ Collection {collName} já existe, pulando...");
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
