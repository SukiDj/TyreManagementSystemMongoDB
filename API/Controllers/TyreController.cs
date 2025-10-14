using Application.Tyres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class TyreController : BaseApiController
    {
        [HttpGet("GetTyres")]
        public async Task<IActionResult> GetTyres()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTyre(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
        }

    }
}