using Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        // Kolekcije
        public IMongoCollection<User> Users { get; }
        public IMongoCollection<Tyre> Tyres { get; }
        public IMongoCollection<Machine> Machines { get; }
        public IMongoCollection<Production> Productions { get; }
        public IMongoCollection<Sale> Sales { get; }
        public IMongoCollection<Client> Clients { get; }
        public IMongoCollection<ActionLog> ActionLogs { get; }

        public MongoDbContext(IOptions<MongoOptions> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);

            // Inicijalizacija kolekcija
            Users = _database.GetCollection<User>("Users");
            Tyres = _database.GetCollection<Tyre>("Tyres");
            Machines = _database.GetCollection<Machine>("Machines");
            Productions = _database.GetCollection<Production>("Productions");
            Sales = _database.GetCollection<Sale>("Sales");
            Clients = _database.GetCollection<Client>("Clients");
            ActionLogs = _database.GetCollection<ActionLog>("ActionLogs");

            // Opcionalno: Kreiranje indeksa (npr. po UserName ili Email)
            //CreateIndexes();
        }

        private void CreateIndexes()
        {
            var userKeys = Builders<User>.IndexKeys.Ascending(u => u.Username);
            Users.Indexes.CreateOne(new CreateIndexModel<User>(userKeys, new CreateIndexOptions { Unique = true }));
        }
    }
}
