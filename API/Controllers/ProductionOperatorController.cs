using Application.Productions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ProductionOperatorController : BaseApiController
    {
        [HttpPost("registerProduction")]
        public async Task<IActionResult> RegisterProduction(RegisterProduction.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetProductionHistory()
        {
            return HandleResult(await Mediator.Send(new ListProductionHistory.Query()));
        }
    }
}