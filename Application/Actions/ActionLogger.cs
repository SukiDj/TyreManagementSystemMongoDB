using Domain;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Actions
{
    public class ActionLogger
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionLogger(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActionAsync(string actionName, string details)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";

            var log = new ActionLog
            {
                Id = Guid.NewGuid(),
                ActionName = actionName,
                UserId = userId,
                Timestamp = DateTime.UtcNow,
                Details = details
            };

            _context.ActionLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}