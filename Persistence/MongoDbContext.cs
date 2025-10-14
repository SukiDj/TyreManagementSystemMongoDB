// using MongoDB.Bson.Serialization.Conventions;
// using MongoDB.Bson;
// using MongoDB.Driver;
// using Microsoft.Extensions.Options;

// namespace Persistence;

// public interface IMongoDbContext
// {
//     IMongoDatabase Database { get; }
//     IMongoCollection<T> GetCollection<T>(string name);
// }

// public class MongoDbContext : IMongoDbContext
// {
//     public IMongoDatabase Database { get; }

//     public MongoDbContext(IOptions<MongoOptions> options, IMongoClient client)
//     {
//         var opts = options.Value;

//         // Safe default for modern drivers
//         ConventionRegistry.Register(
//             "TmsConventions",
//             new ConventionPack {
//                 new CamelCaseElementNameConvention(),
//                 new IgnoreExtraElementsConvention(true)
//             },
//             _ => true
//         );

//         Database = client.GetDatabase(opts.DatabaseName);
//     }

//     public IMongoCollection<T> GetCollection<T>(string name) => Database.GetCollection<T>(name);
// }


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
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var userKeys = Builders<User>.IndexKeys.Ascending(u => u.Username);
            Users.Indexes.CreateOne(new CreateIndexModel<User>(userKeys, new CreateIndexOptions { Unique = true }));

            var tyreKeys = Builders<Tyre>.IndexKeys.Ascending(t => t.Code);
            Tyres.Indexes.CreateOne(new CreateIndexModel<Tyre>(tyreKeys, new CreateIndexOptions { Unique = true }));

            var clientKeys = Builders<Client>.IndexKeys.Ascending(c => c.Id);
            Clients.Indexes.CreateOne(new CreateIndexModel<Client>(clientKeys, new CreateIndexOptions { Unique = true }));

            var productionKeys = Builders<Production>.IndexKeys.Ascending(p => p.Id);
            Productions.Indexes.CreateOne(new CreateIndexModel<Production>(productionKeys, new CreateIndexOptions { Unique = true }));

            var saleKeys = Builders<Sale>.IndexKeys.Ascending(s => s.Id);
            Sales.Indexes.CreateOne(new CreateIndexModel<Sale>(saleKeys, new CreateIndexOptions { Unique = true }));

            var machineKeys = Builders<Machine>.IndexKeys.Ascending(m => m.Id);
            Machines.Indexes.CreateOne(new CreateIndexModel<Machine>(machineKeys, new CreateIndexOptions { Unique = true }));
        }
    }
}
