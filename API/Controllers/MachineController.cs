using Application.Machines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class MachineController : BaseApiController
    {
        [HttpGet("GetMachines")]
        public async Task<IActionResult> GetMachines()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
    }
}