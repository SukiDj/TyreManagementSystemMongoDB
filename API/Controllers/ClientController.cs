using Application.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ClientController : BaseApiController
    {
        [HttpGet("GetClients")]
        public async Task<IActionResult> GetClients()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
    }
}