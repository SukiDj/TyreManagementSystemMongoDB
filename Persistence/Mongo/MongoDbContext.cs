using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace Persistence.Mongo;

public interface IMongoDbContext
{
    IMongoDatabase Database { get; }
    IMongoCollection<T> GetCollection<T>(string name);
}

public class MongoDbContext : IMongoDbContext
{
    public IMongoDatabase Database { get; }

    public MongoDbContext(IOptions<MongoOptions> options, IMongoClient client)
    {
        var opts = options.Value;

        // Safe default for modern drivers
        ConventionRegistry.Register(
            "TmsConventions",
            new ConventionPack {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            },
            _ => true
        );

        Database = client.GetDatabase(opts.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name) => Database.GetCollection<T>(name);
}
