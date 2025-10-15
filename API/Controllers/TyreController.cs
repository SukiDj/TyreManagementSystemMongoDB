using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain;
using Application.Tyres;

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
        public async Task<IActionResult> GetTyre(string id)
        {
            return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
        }

    }
}
