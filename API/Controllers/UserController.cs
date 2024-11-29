using Api.Controllers;
using Application.Common.Extensions;
using Application.Common.Vms;
using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Unit>> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        return await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
    }

    [HttpGet]
    public async Task<ActionResult<DocumentRootJson<List<UserVm>>>> GetUser(CancellationToken cancellationToken)
    {
        var query = new GetUserQuery();
        return await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
    }
}
