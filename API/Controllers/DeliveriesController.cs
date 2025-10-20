using Application.Deliveries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveriesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Delivery>>> List()
            => HandleResult(await Mediator.Send(new List.Query()));

        [HttpPut("{id}/deliver")]
        public async Task<ActionResult> MarkDelivered(string id)
            => HandleResult(await Mediator.Send(new MarkDelivered.Command { Id = id }));
    }
}
