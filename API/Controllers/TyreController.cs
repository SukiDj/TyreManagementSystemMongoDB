using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain;
using Application.Tyres;

namespace API.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]
    // public class TyreController : ControllerBase
    // {
    //     private readonly ITyreRepository _repo;

    //     public TyreController(ITyreRepository repo)
    //     {
    //         _repo = repo;
    //     }

    //     [HttpGet]
    //     [AllowAnonymous]
    //     public async Task<ActionResult<List<Tyre>>> GetAll(CancellationToken ct)
    //     {
    //         var tyres = await _repo.GetAllAsync(ct);
    //         return Ok(tyres);
    //     }

    //     [HttpGet("{id}")]
    //     [AllowAnonymous]
    //     public async Task<ActionResult<Tyre>> GetById(string id, CancellationToken ct)
    //     {
    //         var tyre = await _repo.GetByIdAsync(id, ct);
    //         if (tyre is null) return NotFound();
    //         return Ok(tyre);
    //     }
    // }
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
