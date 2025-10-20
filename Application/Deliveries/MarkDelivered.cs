using Application.Actions;
using Application.Core;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Persistence;

namespace Application.Deliveries
{
    public class MarkDelivered
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; } = default!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly MongoDbContext _db;
            private readonly ActionLogger _log;
            public Handler(MongoDbContext db, ActionLogger log) { _db = db; _log = log; }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
            {
                var filter = Builders<Domain.Delivery>.Filter.Eq(d => d.Id, request.Id);
                var update = Builders<Domain.Delivery>.Update
                    .Set(d => d.Status, "Delivered");

                var res = await _db.Deliveries.UpdateOneAsync(filter, update, cancellationToken: ct);
                if (res.MatchedCount == 0) return Result<Unit>.Failure("Delivery not found");

                await _log.LogActionAsync("MarkDeliveryDelivered", $"Delivery {request.Id} marked Delivered");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
