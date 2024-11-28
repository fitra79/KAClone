using Api.Controllers;
using Application.Common.Extensions;
using Application.Common.Vms;
using Application.ToDoLists.Commands.Create;
using Application.ToDoLists.Commands.Delete;
using Application.ToDoLists.Commands.Update;
using Application.ToDoLists.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    public class ToDoListController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<DocumentRootJson<List<ToDoListVm>>>> GetDivisions(CancellationToken cancellationToken)
        {
            var query = new GetToDoListQueries();
            return await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreateToDoList([FromBody] CreateToDoListCommand command,
        CancellationToken cancellationToken)
        {
            return await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> UpdateToDoList(
        [BindRequired] string id,
        [FromBody] UpdateToDoListCommand command,
        CancellationToken cancellationToken)
        {
            command.Id = id;
            return await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> DeleteToDoList(
        [BindRequired] string id,
        CancellationToken cancellationToken)
        {
            return await Mediator.Send(new DeleteToDoListCommand { Id = id }, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}