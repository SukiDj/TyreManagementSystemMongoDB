using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[AllowAnonymous]

public class BuggyController : BaseApiController
{
    [HttpGet("not-found")]
    public ActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest();
    }

    [HttpGet("server-error")]
    public ActionResult GetServerError()
    {
        throw new Exception("Kvo sad jebavas");
    }

    [HttpGet("unauthorized")]
    public ActionResult GetUnauthorised()
    {
        return Unauthorized();
    }
}