using Domain;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Persistence;

namespace Application.Actions
{
    public class ActionLogger
    {
        private readonly MongoDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionLogger(MongoDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActionAsync(string actionName, string details)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";

            var log = new ActionLog
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ActionName = actionName,
                UserId = userId,
                Timestamp = DateTime.UtcNow,
                Details = details
            };

            await _context.ActionLogs.InsertOneAsync(log);
        }
    }
}
