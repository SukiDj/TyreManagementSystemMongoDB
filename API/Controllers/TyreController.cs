using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Persistence.Mongo.Repositories;
using Domain;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TyreController : ControllerBase
    {
        private readonly ITyreRepository _repo;

        public TyreController(ITyreRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Tyre>>> GetAll(CancellationToken ct)
        {
            var tyres = await _repo.GetAllAsync(ct);
            return Ok(tyres);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Tyre>> GetById(string id, CancellationToken ct)
        {
            var tyre = await _repo.GetByIdAsync(id, ct);
            if (tyre is null) return NotFound();
            return Ok(tyre);
        }
    }
}
