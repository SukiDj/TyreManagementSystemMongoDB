using MongoDB.Driver;
using Persistence.Mongo;
using Domain; // Tyre.cs is in your Domain project

namespace Persistence.Mongo.Repositories
{
    public interface ITyreRepository
    {
        Task<Tyre?> GetByIdAsync(string id, CancellationToken ct = default);
        Task<List<Tyre>> GetAllAsync(CancellationToken ct = default);
        Task CreateAsync(Tyre tyre, CancellationToken ct = default);
        Task UpdateAsync(string id, Tyre tyre, CancellationToken ct = default);
        Task DeleteAsync(string id, CancellationToken ct = default);
    }

    public class TyreRepository : ITyreRepository
    {
        private readonly IMongoCollection<Tyre> _tyres;

        public TyreRepository(IMongoDbContext ctx)
        {
            _tyres = ctx.GetCollection<Tyre>("tyres"); // collection name
        }

        public Task<Tyre?> GetByIdAsync(string id, CancellationToken ct = default) =>
            _tyres.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

        public Task<List<Tyre>> GetAllAsync(CancellationToken ct = default) =>
            _tyres.Find(FilterDefinition<Tyre>.Empty).ToListAsync(ct);

        public Task CreateAsync(Tyre tyre, CancellationToken ct = default) =>
            _tyres.InsertOneAsync(tyre, cancellationToken: ct);

        public Task UpdateAsync(string id, Tyre tyre, CancellationToken ct = default) =>
            _tyres.ReplaceOneAsync(x => x.Id == id, tyre, cancellationToken: ct);

        public Task DeleteAsync(string id, CancellationToken ct = default) =>
            _tyres.DeleteOneAsync(x => x.Id == id, ct);
    }
}
