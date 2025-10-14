using Application.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ActionLogController : BaseApiController
    {
        [HttpGet("GetActions")]
        public async Task<IActionResult> GetActions()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
    }
}